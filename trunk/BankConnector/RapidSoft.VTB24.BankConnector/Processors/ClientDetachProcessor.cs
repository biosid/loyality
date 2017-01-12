namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    using ClientProfile = RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using Processing = RapidSoft.Loaylty.Processing.ProcessingService;

    public class ClientDetachProcessor : ProcessorBase
    {
        private readonly ClientProfile.ClientProfileService clientProfileService;

        private readonly IOrderManagementService orderManagementService;

        private readonly Processing.ProcessingService processingService;

        private readonly ISecurityWebApi securityWebApi;

        public ClientDetachProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IOrderManagementService orderManagementService,
            ClientProfile.ClientProfileService clientProfileService,
            Processing.ProcessingService processingService,
            ISecurityWebApi securityWebApi)
            : base(logger, uow)
        {
            this.orderManagementService = orderManagementService;
            this.clientProfileService = clientProfileService;
            this.processingService = processingService;
            this.securityWebApi = securityWebApi;
        }

        public void PrepareClientsToDelete()
        {
            var clientsToCheckList =
                this.Uow.ClientForDeletionRepository.GetAll()
                    .Where(
                        x =>
                            (x.Status != (int)ClientDetachStatus.Success || x.Status == null)
                                && x.SendEtlSessionId == null)
                    .ToList();
            this.Logger.InfoFormat(
                "Записей в списке на отключение, подлежащих проверке возможности отключения: {0}",
                clientsToCheckList.Count);

            foreach (var client in clientsToCheckList)
            {
                try
                {
                    this.Logger.InfoFormat("Проверка возможности удаления клиента ({0})", client.ExternalClientId);

                    client.Status = (int)this.GetDeleteClientStatus(client);

                    if (client.Status == (int)ClientDetachStatus.ReadyForProcessing)
                    {
                        client.Status = (int)ClientDetachStatus.Success;
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.ErrorFormat(
                        ex,
                        "Ошибка при проверке возможности отключения клиента с ClientId: {0}",
                        client.ExternalClientId);
                    client.Status = (int)ClientDetachStatus.UnknkownError;
                }
            }

            this.Logger.Info("Сохранение изменений в БД (ClientForDeletionRepository)");
            this.Uow.Save();
        }

        public void AddDetachRequest(string clientId)
        {
            var user = this.GetUserFromSecurity(clientId);

            if (user == null)
            {
                throw new Exception(
                    string.Format("Клиент ({0}) не найден в Security", clientId));
            }

            this.LockInSecurity(user.PhoneNumber);

            var repository = this.Uow.ClientForDeletionRepository;

            if (repository.GetAll().Any(deleted => deleted.ExternalClientId == clientId))
            {
                return;
            }

            var clientProfile = this.GetClientProfileByExternalId(clientId);

            var sessionId = Guid.NewGuid();

            if (clientProfile.ClientStatus != (int)ClientProfileClientStatusCodes.Created)
            {
                this.LockInClientProfile(clientId, sessionId);

                this.LockInProcessing(clientId, sessionId);
            }

            if (clientProfile.Phones == null || clientProfile.Phones.Length == 0)
            {
                throw new Exception(
                    string.Format("У клиента ({0}) не указано ни одного телефона", clientProfile.ClientId));
            }

            var primaryPhone = clientProfile.Phones.SingleOrDefault(x => x.IsPrimary);

            if (primaryPhone == null || string.IsNullOrEmpty(primaryPhone.PhoneNumber))
            {
                throw new Exception(
                    string.Format(
                        "У клиента ({0}) не указан основной номер телефона", clientProfile.ClientId));
            }

            var client = new ClientForDeletion
                         {
                             InsertEtlSessionId = sessionId,
                             ExternalClientId = clientId,
                             Status = null,
                             MobilePhone = primaryPhone.PhoneNumber,
                         };

            repository.Add(client);
            this.Uow.Save();
        }

        public void DetachClients()
        {
            var etlSessionId = new Guid(Logger.EtlSessionId);

            var clients =
                (from clForDelResp in
                     this.Uow.ClientForDeletionResponseRepository.GetAll()
                 join clForDel in this.Uow.ClientForDeletionRepository.GetAll() on clForDelResp.ClientId equals
                     clForDel.ExternalClientId into joinRes
                 from leftJoinRes in joinRes.DefaultIfEmpty()
                 where clForDelResp.DetachStatus == null && clForDelResp.EtlSessionId == etlSessionId
                 select new { clForDel = leftJoinRes, clForDelResp }).ToList();

            this.Logger.InfoFormat("Клиентов на отключение: {0}", clients.Count(x => x.clForDelResp.Status == ClientForDeletionResponseStatuses.Confirm));
            this.Logger.InfoFormat("Клиентов на отмену отключения: {0}", clients.Count(x => x.clForDelResp.Status == ClientForDeletionResponseStatuses.Cancel));

            foreach (var client in clients)
            {
                var clientId = client.clForDelResp.ClientId;

                DetachOperationResult result;

                switch (client.clForDelResp.Status)
                {
                    case ClientForDeletionResponseStatuses.Confirm:
                        this.Logger.InfoFormat("Отключение клиента {0}", clientId);
                        result = this.DetachClient(client.clForDel);
                        break;

                    case ClientForDeletionResponseStatuses.Cancel:
                        this.Logger.InfoFormat("Отмена отключения клиента {0}", clientId);
                        result = this.CancelDetach(client.clForDel, etlSessionId);
                        break;

                    default:
                        this.Logger.WarnFormat("Неподдерживаемый статус подтверждения {0} для клиента {1}", client.clForDelResp.Status, clientId);
                        result = new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, "неизвестный статус подтверждения отключения");
                        break;
                }

                client.clForDelResp.DetachStatus = (int?)result.Status;
                client.clForDelResp.Message = result.Message;
            }

            this.Logger.Info("Сохранение изменений в БД");

            this.Uow.Save();
        }

        private DetachOperationResult CancelDetach(ClientForDeletion client, Guid etlSessionId)
        {
            if (client == null)
            {
                this.Logger.Error("Заказ на отключение не найден");

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, "Заказ на отключение не найден");
            }

            var clientId = client.ExternalClientId;

            try
            {
                var user = this.GetUserFromSecurity(clientId);

                string msg;
                if (user == null)
                {
                    msg = string.Format("Клиент ({0}) не найден", clientId);
                    this.Logger.ErrorFormat(msg);

                    return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, msg);
                }

                this.UnlockInSecurity(user.PhoneNumber);

                var clientProfile = this.GetClientProfileByExternalId(clientId);

                if (clientProfile.ClientStatus != (int)ClientProfileClientStatusCodes.Created)
                {
                    this.UnlockInClientProfile(clientId, etlSessionId);

                    this.UnlockInProcessing(clientId, etlSessionId);
                }

                this.Uow.ClientForDeletionRepository.Delete(client);

                msg = string.Format("Отключение отменено ({0})", clientId);
                this.Logger.InfoFormat(msg);

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Success);
            }
            catch (Exception ex)
            {
                this.Logger.ErrorFormat(ex, "Ошибка отмены отключения {0}", clientId);

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, "Неизвестная ошибка");
            }
        }

        private DetachOperationResult DetachClient(ClientForDeletion client)
        {
            if (client == null)
            {
                this.Logger.Error("Заказ на отключение не найден");

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, "Заказ на отключение не найден");
            }

            try
            {
                var user = this.GetUserFromSecurity(client.ExternalClientId);

                if (user == null)
                {
                    var msg = string.Format("Клиент ({0}) не найден", client.ExternalClientId);
                    this.Logger.ErrorFormat(msg);

                    return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, msg);
                }

                this.DeleteFromSecurity(user.PhoneNumber);

                this.DeleteFromProcessing(client);

                this.DeleteFromClientProfile(client);

                this.Logger.InfoFormat("Клиент отключен ({0})", client.ExternalClientId);

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Success);
            }
            catch (Exception ex)
            {
                this.Logger.ErrorFormat(ex, "Ошибка отключения {0}", client.ExternalClientId);

                return new DetachOperationResult(ClientForDeletionResponseDetachStatus.Error, "Неизвестная ошибка");
            }
        }

        private ClientDetachStatus GetDeleteClientStatus(ClientForDeletion client)
        {
            var clientProfile = this.GetClientProfileByExternalId(client.ExternalClientId);

            if (clientProfile.ClientStatus != (int)ClientProfileClientStatusCodes.Blocked
                && clientProfile.ClientStatus != (int)ClientProfileClientStatusCodes.Created)
            {
                var message =
                    string.Format(
                        "Пользователь c ClientId ({0}) не находится в статусе, разрешающем удаление, текущий статус: ({1})",
                        clientProfile.ClientId,
                        clientProfile.ClientStatus);
                this.Logger.InfoFormat(
                    "Ошибка при проверке статуса пользователя ({1}) в ClientProfile: {0}",
                    message,
                    client.ExternalClientId);
            }

            var hasNonTerminatedOrders = this.orderManagementService.HasNonterminatedOrders(client.ExternalClientId);

            if (hasNonTerminatedOrders.ResultCode != 0)
            {
                throw new Exception(
                    string.Format(
                        "Ошибка обращения для клиента {0} к сервису OrderManagementService: {1} - {2}",
                        client.ExternalClientId,
                        hasNonTerminatedOrders.ResultCode,
                        hasNonTerminatedOrders.ResultDescription));
            }

            this.Logger.InfoFormat(
                "Результат проверки наличия незавершенных заказов: {0}",
                hasNonTerminatedOrders.HasOrders ? "присутствуют" : "отсутствуют");
            return hasNonTerminatedOrders.HasOrders
                ? ClientDetachStatus.HasNonTerminatedOrders
                : ClientDetachStatus.ReadyForProcessing;
        }

        #region взаимодействие с безопасностью

        private User GetUserFromSecurity(string clientId)
        {
            var users = this.securityWebApi.BatchResolveUsersByClientId(new[] { clientId });

            if (users == null)
            {
                throw new Exception("SecurityWebAPI.BatchResolveUsersByClientId вернул null");
            }

            User user;
            users.TryGetValue(clientId, out user);
            return user;
        }

        private void LockInSecurity(string login)
        {
            this.Logger.InfoFormat(string.Format("Блокировка {0} в Security", login));

            try
            {
                this.securityWebApi.DisableUser(login);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка блокировки пользователя {0} в Security.", login), e);
            }
        }

        private void UnlockInSecurity(string login)
        {
            this.Logger.InfoFormat(string.Format("Разблокирование {0} в Security", login));

            try
            {
                this.securityWebApi.EnableUser(login);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка разблокирования пользователя {0} в Security.", login), e);
            }
        }

        private void DeleteFromSecurity(string login)
        {
            this.Logger.InfoFormat("Выполнение удаления в SecurityService");

            try
            {
                this.securityWebApi.DeleteUser(login);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка удаления пользователя {0} из SecurityWebAPI.", login), e);
            }
        }

        #endregion

        #region взаимодействие с процессингом

        private void LockInProcessing(string clientId, Guid sessionId)
        {
            var processingBlockRequest = new Processing.BatchLockClientsRequestType
            {
                EtlSessionId = sessionId.ToString(),
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                LockFacts = new[]
                {
                    new Processing.BatchLockClientsRequestTypeLockFact
                    {
                        ClientExternalId = clientId
                    }
                }
            };

            var blockProcessingResponse =
                this.processingService.BatchLockClients(new Processing.BatchLockClientsRequest(processingBlockRequest));

            if (blockProcessingResponse.Response == null || blockProcessingResponse.Response.StatusCode != 0)
            {
                throw new Exception("Ошибка при вызове сервиса Processing для блокировки клиента " + clientId);
            }

            if (blockProcessingResponse.Response.ClientLockResults == null
                || blockProcessingResponse.Response.ClientLockResults.Length == 0
                || blockProcessingResponse.Response.ClientLockResults[0].StatusCode != 0)
            {
                throw new Exception(
                    "Ответ сервиса Processing содержит неверный результат блокировки клиента: " + clientId);
            }
        }

        private void UnlockInProcessing(string clientId, Guid sessionId)
        {
            var request = new Processing.BatchActivateClientsRequestType
            {
                EtlSessionId = sessionId.ToString(),
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                ActivationFacts = new[]
                {
                    new Processing.BatchActivateClientsRequestTypeActivationFact
                    {
                        ClientExternalId = clientId
                    }
                }
            };

            var response =
                this.processingService.BatchActivateClients(new Processing.BatchActivateClientsRequest(request));

            if (response == null || response.Response == null || response.Response.StatusCode != 0)
            {
                throw new Exception("Ошибка при вызове сервиса Processing для разблокировки клиента " + clientId);
            }

            if (response.Response.ClientActivationResults == null
                || response.Response.ClientActivationResults.Length == 0
                || response.Response.ClientActivationResults[0].StatusCode != 0)
            {
                throw new Exception(
                    "Ответ сервиса Processing содержит неверный результат разблокировки клиента: " + clientId);
            }
        }

        private void DeleteFromProcessing(ClientForDeletion clientForDetach)
        {
            var processingRequest = new Processing.BatchDeleteClientsRequestType
            {
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                EtlSessionId = this.Logger.EtlSessionId,
                DeletionFacts = new[]
                {
                    new Processing.BatchDeleteClientsRequestTypeDeletionFact
                    {
                        ClientExternalId = clientForDetach.ExternalClientId
                    }
                }
            };

            this.Logger.InfoFormat("Выполнение удаления в Processing");
            var processingResponse =
                this.processingService.BatchDeleteClients(new Processing.BatchDeleteClientsRequest(processingRequest))
                    .Response;

            if (processingResponse.StatusCode != 0)
            {
                throw new Exception(
                    string.Format(
                        "Ошибка обращения для клиента {0} к сервису Processing: {1}",
                        clientForDetach.ExternalClientId,
                        processingResponse.StatusCode));
            }

            if (processingResponse.ClientDeletionResults == null || processingResponse.ClientDeletionResults.Length < 1
                || processingResponse.ClientDeletionResults[0].StatusCode != 0)
            {
                throw new Exception(
                    string.Format(
                        "Ошибка обращения для клиента {0} к сервису Processing. Неверное значение ClientDeletionResults",
                        clientForDetach.ExternalClientId));
            }
        }

        #endregion

        #region взаимодействие с профилем

        private ClientProfile.GetClientProfileFullResponseTypeClientProfile GetClientProfileByExternalId(string externalClientId)
        {
            var request = new ClientProfile.BatchGetClientsByExternalIdRequestType
            {
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                ReqClientsIdentifiers = new[]
                {
                    new ClientProfile.BatchGetClientsByExternalIdRequestTypeReqClientIdentifier
                    {
                        ClientExternalId = externalClientId
                    }
                }
            };

            var clientsWithStatuses =
                this.clientProfileService.BatchGetClientsByExternalId(
                    new ClientProfile.BatchGetClientsByExternalIdRequest(request)).Response;

            if (clientsWithStatuses.StatusCode != 0)
            {
                throw new ApplicationException(
                    string.Format(
                        "Ошибка получения ClientId для клиента c ExternalClientId {0} из ClientProfile: {1}",
                        externalClientId,
                        clientsWithStatuses.StatusCode));
            }

            if (clientsWithStatuses.ResClientsIdentifiers == null
                || clientsWithStatuses.ResClientsIdentifiers.Length == 0)
            {
                throw new ApplicationException(
                    string.Format(
                        "Ошибка получения ClientId для клиента c ExternalClientId {0} из ClientProfile. Неверное значение ResClientsIdentifiers",
                        externalClientId));
            }

            if (clientsWithStatuses.ResClientsIdentifiers[0].StatusCode != 0)
            {
                throw new ApplicationException(
                    string.Format(
                        "Получение ClientId для клиента с ExternalClientId ({0}) завершилось с ошибкой ({1} - {2})",
                        externalClientId,
                        clientsWithStatuses.ResClientsIdentifiers[0].StatusCode,
                        clientsWithStatuses.ResClientsIdentifiers[0].Error));
            }

            var requestFull = new ClientProfile.GetClientProfileFullRequestType
            {
                ClientId = clientsWithStatuses.ResClientsIdentifiers[0].ClientId
            };

            var responseFull =
                this.clientProfileService.GetClientProfileFull(
                    new ClientProfile.GetClientProfileFullRequest(requestFull));

            if (responseFull.Response == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "Запрос пофиля пользователя c ExternalClientId ({0}) и ClientId ({1}) в ClientProfile не вернул ответа",
                        externalClientId,
                        requestFull.ClientId));
            }

            if (responseFull.Response.StatusCode != 0)
            {
                throw new ApplicationException(
                    string.Format(
                        "Запрос профиля пользователя c ExternalClientId ({0}) и ClientId ({1}) в ClientProfile вернул ошибку ({2} - {3})",
                        externalClientId,
                        requestFull.ClientId,
                        responseFull.Response.StatusCode,
                        responseFull.Response.Error));
            }

            if (responseFull.Response.ClientProfile == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "Запрос профиля пользователя c ExternalClientId ({0}) и ClientId ({1}) в ClientProfile вернул успешный статус, но не вернул профиля пользователя",
                        externalClientId,
                        requestFull.ClientId));
            }

            return responseFull.Response.ClientProfile;
        }

        private void LockInClientProfile(string clientId, Guid sessionId)
        {
            var blockRequest = new ClientProfile.BatchLockClientsRequestType
            {
                EtlSessionId = sessionId.ToString(),
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                LockFacts = new[]
                {
                    new ClientProfile.BatchLockClientsRequestTypeLockFact
                    {
                        ClientExternalId = clientId
                    }
                },
            };

            var blockResponse =
                this.clientProfileService.BatchLockClients(new ClientProfile.BatchLockClientsRequest(blockRequest));

            if (blockResponse.Response == null || blockResponse.Response.StatusCode != 0)
            {
                throw new Exception("Ошибка при вызове сервиса ClientProfile для блокировки клиента " + clientId);
            }

            if (blockResponse.Response.ClientLockResults == null || blockResponse.Response.ClientLockResults.Length == 0
                || blockResponse.Response.ClientLockResults[0].StatusCode != 0)
            {
                throw new Exception(
                    "Ответ сервиса ClientProfile содержит неверный результат блокировки клиента: " + clientId);
            }
        }

        private void UnlockInClientProfile(string clientId, Guid sessionId)
        {
            var request = new ClientProfile.BatchActivateClientsRequestType
            {
                EtlSessionId = sessionId.ToString(),
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                ClientActivationFacts = new[]
                {
                    new ClientProfile.BatchActivateClientsRequestTypeClientActivationFact
                    {
                        ClientExternalId = clientId
                    }
                }
            };

            var response =
                this.clientProfileService.BatchActivateClients(new ClientProfile.BatchActivateClientsRequest(request));

            if (response == null || response.Response == null || response.Response.StatusCode != 0)
            {
                throw new Exception("Ошибка при вызове сервиса ClientProfile для разблокировки клиента " + clientId);
            }

            if (response.Response.ClientActivationResults == null
                || response.Response.ClientActivationResults.Length == 0
                || response.Response.ClientActivationResults[0].StatusCode != 0)
            {
                throw new Exception("Ответ сервиса ClientProfile содержит неверный результат разблокировки клиента " + clientId);
            }
        }

        private void DeleteFromClientProfile(ClientForDeletion clientForDetach)
        {
            var clientProfileRequest = new ClientProfile.BatchDeleteClientsRequestType
            {
                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                EtlSessionId = this.Logger.EtlSessionId,
                DeletionFacts = new[]
                {
                    new ClientProfile.BatchDeleteClientsRequestTypeDeletionFact
                    {
                        ClientExternalId = clientForDetach.ExternalClientId
                    }
                }
            };

            this.Logger.InfoFormat("Выполнение удаления в ClientProfile");
            var clientProfileResponse =
                this.clientProfileService.BatchDeleteClients(
                    new ClientProfile.BatchDeleteClientsRequest(clientProfileRequest)).Response;

            if (clientProfileResponse.StatusCode != 0)
            {
                throw new Exception(
                    string.Format(
                        "Ошибка обращения для клиента {0} к сервису ClientProfile: {1}",
                        clientForDetach.ExternalClientId,
                        clientProfileResponse.StatusCode));
            }

            if (clientProfileResponse.ClientDeletionResults == null
                || clientProfileResponse.ClientDeletionResults.Length < 1
                || clientProfileResponse.ClientDeletionResults[0].StatusCode != 0)
            {
                throw new Exception(
                    string.Format(
                        "Ошибка обращения для клиента {0} к сервису ClientProfile. Неверное значение ClientDeletionResults",
                        clientForDetach.ExternalClientId));
            }
        }

        #endregion

        private class DetachOperationResult
        {
            public DetachOperationResult(ClientForDeletionResponseDetachStatus? status, string msg = null)
            {
                Status = status;
                Message = msg;
            }

            public ClientForDeletionResponseDetachStatus? Status { get; private set; }

            public string Message { get; private set; }
        }
    }
}