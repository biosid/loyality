namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Processors.MapHelpers;
    using RapidSoft.VTB24.BankConnector.Service;

    using ClientProfile = RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using Processing = RapidSoft.Loaylty.Processing.ProcessingService;
    using Security = RapidSoft.VTB24.Site.SecurityWebApi;

    public class ClientRegistrator : ProcessorBase
    {
        private readonly ClientProfile.ClientProfileService clientProfileService;

        private readonly int loyaltyProgramId;

        private readonly Processing.ProcessingService processingService;

        private readonly Security.ISecurityWebApi securityWebApi;

        private readonly ITargetAudienceService targetAudienceService;

        public ClientRegistrator(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            Processing.ProcessingService processingService,
            ClientProfile.ClientProfileService clientProfileService,
            Security.ISecurityWebApi securityWebApi,
            ITargetAudienceService targetAudienceService)
            : base(logger, uow)
        {
            this.loyaltyProgramId = ConfigHelper.LoyaltyProgramId;
            this.processingService = processingService;
            this.clientProfileService = clientProfileService;
            this.securityWebApi = securityWebApi;
            this.targetAudienceService = targetAudienceService;
        }

        public void RegisterClients()
        {
            var totalProcessed = 0;
            var responseSessionId = Guid.Parse(this.Logger.EtlSessionId);
            int batchCount = 0;

            do
            {
                var batch =
                    (from regResponse in
                        this.Uow.ClientForRegistrationResponseRepository.GetBySessionId(responseSessionId)
                     join regRequest in this.Uow.ClientForRegistrationRepository.GetAll().Where(c => !c.IsDeleted) on regResponse.ClientId equals
                         regRequest.ClientId into joinRes
                     from leftJoinRes in joinRes.DefaultIfEmpty()
                     orderby leftJoinRes.ClientId
                     select new ClientRegistrationJoin
                            {
                                RegRequest = leftJoinRes, 
                                RegResponse = regResponse
                            }).Skip(totalProcessed).Take(ConfigHelper.BatchSize).ToList();

                // Отмечаем не найденные заявки
                batch.ForEach(
                    batchItem =>
                    {
                        if (batchItem.RegRequest == null)
                        {
                            batchItem.RegResponse.RegStatus = RegClientStatuses.RequestNotFound;
                        }
                    });
                this.Uow.Save();

                // Обрабатываем все отмены банка по регистрации
                var cancelledClients =
                    batch.Where(
                        batchItem =>
                            batchItem.RegResponse.RegStatus == null
                                && batchItem.RegResponse.Status != (int?)RegisterClientStatus.Success).ToList();

                this.DenyRegistration(cancelledClients);

                // Отмечаем не валидные сегменты
                foreach (
                    var invalidClient in
                        batch.Where(
                            x =>
                                x.RegResponse.RegStatus == null
                                    && x.RegResponse.Status == (int)RegisterClientStatus.Success
                                    && !IsValidSegment(x.RegResponse.Segment)))
                {
                    this.Logger.InfoFormat(
                        "Прислан не корректный сегмент клиент {0}", invalidClient.RegResponse.ClientId);
                    invalidClient.RegResponse.RegStatus = RegClientStatuses.InvalidSegment;
                }

                this.Uow.Save();

                // NOTE: Проверяем если среди отобранных уже зарегистрированные
                var clients = batch.Where(batchItem => batchItem.RegResponse.RegStatus == null
                    && batchItem.RegResponse.Status == (int)RegisterClientStatus.Success);
                this.MarkAlreadyRegistered(clients.ToList());

                // Регистрируем валидных клиентов
                var validClients =
                    batch.Where(
                        batchItem =>
                            batchItem.RegResponse.RegStatus == null
                                && batchItem.RegResponse.Status == (int)RegisterClientStatus.Success).ToList();

                this.Logger.InfoFormat(
                    "Выбрано записей с признаком успешной регистрации на стороне банка: {0}", validClients.Count);

                var clientProfileRequest = this.PrepareClientProfileRequest(validClients);

                this.Logger.Info("Invoke ClientProfileService create clients");

                var createClientsResponse = this.ClientProfileCreateClients(clientProfileRequest, validClients);

                this.Logger.Info("Invoke ProcessingService create clients");
                var responseProcessing = this.ProcessingCreateClients(createClientsResponse, validClients);

                this.Logger.Info("Invoke SecurityWebApiService create clients");
                var loyaltyRegisteredClientsIds = this.SecurityWebApiCreateUsers(
                    responseProcessing, clientProfileRequest, validClients);

                this.Logger.Info(
                    "Формируем набор сегментов с идентификаторами клиента для привязки в компоненте \"Акции\" и регистрируем");
                var segments =
                    validClients.Where(x => loyaltyRegisteredClientsIds.Contains(x.RegResponse.ClientId))
                                .GroupBy(x => x.RegResponse.Segment)
                                .Select(group => group.ToSegment(x => x.RegResponse.ClientId));

                this.targetAudienceService.CallAssignClientSegment(segments, this.Logger);
                this.Logger.Info("Регистрация в сегментах выполнена");

                // Отмечаем всех успешно зарегистрированных
                validClients.Where(x => loyaltyRegisteredClientsIds.Contains(x.RegResponse.ClientId)).ForEach(c => c.RegResponse.RegStatus = RegClientStatuses.Registred);

                // Отмечаем всех обработанных
                batch.ForEach(
                    batchItem =>
                    {
                        if (batchItem.RegRequest != null)
                        {
                            batchItem.RegRequest.ResponseSessionId = responseSessionId;
                            batchItem.RegRequest.IsDeleted = true;
                        }
                    });                
                this.Uow.Save();

                totalProcessed += batch.Count;

                this.Logger.Info("Total processed - " + totalProcessed);
                batchCount = batch.Count;
            }
            while (batchCount > 0);

            this.Logger.Info("Clients registration completed");
        }

        private static bool IsValidSegment(int? segment)
        {
            var goodSegments = new[]
                               {
                                   (int)ClientSegment.noVIP, (int)ClientSegment.VIP
                               };

            return segment.HasValue && goodSegments.Contains(segment.Value);
        }

        private void DenyRegistration(List<ClientRegistrationJoin> cancelledClients)
        {
            foreach (var cancelledClient in cancelledClients)
            {
                this.Logger.Info(
                    string.Format(
                        "Вызов securityWebApi.DenyRegistrationRequest клиент {0}", cancelledClient.RegResponse.ClientId));

                try
                {
                    this.securityWebApi.DenyRegistrationRequest(
                        new Security.DenyRegistrationRequestOptions
                        {
                            PhoneNumber = cancelledClient.RegRequest.MobilePhone,
                            RegistrationRequestBankStatus = cancelledClient.RegResponse.Status
                        });
                    cancelledClient.RegResponse.RegStatus = RegClientStatuses.Cancelled;
                }
                catch (Exception e)
                {
                    this.Logger.Error(
                        string.Format(
                            "Ошибка вызова securityWebApi.DenyRegistrationRequest клиент {0}",
                            cancelledClient.RegResponse.ClientId),
                        e);
                    cancelledClient.RegResponse.RegStatus = RegClientStatuses.Error;
                }
            }

            this.Uow.Save();
        }

        private void MarkAlreadyRegistered(List<ClientRegistrationJoin> validClients)
        {
            try
            {
                var clientPhones = validClients.Select(x => x.RegRequest.MobilePhone).ToArray();

                var usersByPhone = this.securityWebApi.BatchResolveUsersByPhone(clientPhones);

                if (usersByPhone == null)
                {
                    throw new Exception("SecurityWebApi.BatchResolveUsersByPhone response returns with null");
                }

                var existedClients =
                    validClients.Join(
                        usersByPhone.Where(x => x.Value != null), 
                        forRegistration => forRegistration.RegRequest.MobilePhone, 
                        clientPair => clientPair.Key, 
                        (forRegistration, clientPair) => forRegistration).ToList();

                existedClients.ForEach(x => { x.RegResponse.RegStatus = RegClientStatuses.AlreadyRegistred; });

                if (existedClients.Count > 0)
                {
                    this.Logger.InfoFormat("Уже зарегистрированных клиентов: {0}", existedClients.Count);

                    this.Uow.Save();
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Ошибка при проверке существования клиентов в Security", ex);
                throw;
            }
        }

        private List<string> SecurityWebApiCreateUsers(Processing.BatchCreateClientsResponseType responseProcessing, ClientProfile.BatchCreateClientsRequestType clientProfileRequest, List<ClientRegistrationJoin> validClients)
        {
            var processingRegistered =
                responseProcessing.ClientRegistrationResults.Where(x => x.StatusCode == 0).ToList();

            this.Logger.InfoFormat(
                "Старт выполнения вызовов SecurityService при регистрации клиентов, записей для обработки: ({0})", 
                processingRegistered.Count);

            var successfullyRegistered = new List<string>();

            foreach (var account in processingRegistered)
            {
                var phone =
                    clientProfileRequest.ClientRegistrationFacts.Single(y => y.ClientId == account.ClientId).MobilePhone;

                try
                {
                    var createUserOptions = new Security.CreateUserOptions
                                            {
                                                ClientId = account.ClientId, 
                                                PhoneNumber = phone, 
                                                RegistrationType = Security.RegistrationType.SiteRegistration
                                            };
                    this.securityWebApi.CreateUser(createUserOptions);
                    successfullyRegistered.Add(account.ClientId);
                    this.Logger.InfoFormat(
                        "Клиент ({0}) с номером телефона ({1}) успешно зарегистрирован", account.ClientId, phone);
                }
                catch (Exception ex)
                {
                    this.Logger.ErrorFormat(
                        ex, 
                        "Ошибка при вызове SecurityService для клиента ({0}) с номером телефона ({1})", 
                        account.ClientId, 
                        phone);
                    var clientForRegistration = validClients.Single(t => t.RegRequest.ClientId == account.ClientId);
                    clientForRegistration.RegResponse.RegStatus = RegClientStatuses.Error;
                }
            }
            
            this.Logger.InfoFormat("Выполнение вызовов SecurityService завершено");
            return successfullyRegistered;
        }

        private Processing.BatchCreateClientsResponseType ProcessingCreateClients(
            ClientProfile.BatchCreateClientsResponseType responseClientProfile, 
            List<ClientRegistrationJoin> responseList)
        {
            Processing.BatchCreateClientsResponseType responseProcessing = null;

            var processingRequest = new Processing.BatchCreateClientsRequestType
                                    {
                                        EtlSessionId = this.Logger.EtlSessionId, 
                                        LoyaltyProgramId = this.loyaltyProgramId, 
                                    };

            var findTrouble = false;
            try
            {
                var processingRequestList =
                    responseClientProfile.ClientRegistrationResults.Where(x => x.StatusCode == 0)
                                         .Select(x => x.ToBatchCreateClientsRequestTypeClientRegistrationFact());

                processingRequest.ClientRegistrationFacts = processingRequestList.ToArray();

                this.Logger.InfoFormat(
                    "Вызов сервиса Processing при регистрации клиентов, записей для обработки: ({0})", 
                    processingRequest.ClientRegistrationFacts.Count());
                responseProcessing =
                    this.processingService.BatchCreateClients(
                        new Processing.BatchCreateClientsRequest(processingRequest)).Response;
                if (responseProcessing.StatusCode != 0)
                {
                    throw new Exception(
                        string.Format(
                            "Сервис Processing при регистрации участников вернул ошибочный статус({0})", 
                            responseProcessing.StatusCode));
                }

                if (responseProcessing.ClientRegistrationResults != null)
                {
                    this.Logger.InfoFormat(
                        "Cервис Processing при регистрации клиентов вернул успешный статус, количество зарегистрированных клиентов в ответе сервиса: ({0})", 
                        responseProcessing.ClientRegistrationResults.Count());

                    responseProcessing.ClientRegistrationResults.ToList().ForEach(
                        x =>
                        {
                            if (x.StatusCode != 0)
                            {
                                const string Mess =
                                    "Регистрация участника в Processing ({0}) c ExternalId ({1}) завершилась с ошибкой ({2}), код ошибки ({3})";
                                this.Logger.ErrorFormat(Mess, x.ClientId, x.ClientExternalId, x.Error, x.StatusCode);

                                // TODO: Как протестить?
                                findTrouble = true;
                                var clientForRegistration =
                                    responseList.Single(t => t.RegResponse.ClientId == x.ClientId);
                                clientForRegistration.RegResponse.Status = RegClientStatuses.Error;
                            }
                            else
                            {
                                const string Mess =
                                    "Регистрация участника в Processing ({0}) c ExternalId ({1}) успешно завершена";
                                this.Logger.InfoFormat(Mess, x.ClientId, x.ClientExternalId);
                            }
                        });
                }
                else
                {
                    this.Logger.InfoFormat("Запрос к Processing не вернул массива записей");
                    responseProcessing.ClientRegistrationResults =
                        new Processing.BatchCreateClientsResponseTypeClientRegistrationResult[0];
                }

                if (findTrouble)
                {
                    this.Uow.Save();
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Ошибка при вызове ProcessingService", ex);
                throw;
            }

            return responseProcessing;
        }

        private ClientProfile.BatchCreateClientsResponseType ClientProfileCreateClients(
            ClientProfile.BatchCreateClientsRequestType clientProfileRequest, List<ClientRegistrationJoin> responseList)
        {
            if (clientProfileRequest == null)
            {
                throw new ArgumentNullException("clientProfileRequest");
            }

            if (clientProfileRequest.ClientRegistrationFacts == null
                || !clientProfileRequest.ClientRegistrationFacts.Any())
            {
                this.Logger.Info(
                    "Вызов сервиса ClientProfile при регистрации клиентов не выполняем так как пакет пустой");
                return new ClientProfile.BatchCreateClientsResponseType
                       {
                           ClientRegistrationResults =
                               new ClientProfile.BatchCreateClientsResponseTypeClientRegistrationResult[0], 
                           StatusCode = 0
                       };
            }

            var findTrouble = false;
            ClientProfile.BatchCreateClientsResponseType responseClientProfile;

            try
            {
                this.Logger.InfoFormat(
                    "Вызов сервиса ClientProfile при регистрации клиентов, записей для обработки: ({0})", 
                    clientProfileRequest.ClientRegistrationFacts.Count());
                responseClientProfile =
                    this.clientProfileService.BatchCreateClients(
                        new ClientProfile.BatchCreateClientsRequest(clientProfileRequest)).Response;

                if (responseClientProfile.StatusCode != 0)
                {
                    throw new Exception(
                        string.Format(
                            "Сервис ClientProfile при регистрации участников вернул ошибочный статус({0})", 
                            responseClientProfile.StatusCode));
                }

                if (responseClientProfile.ClientRegistrationResults == null)
                {
                    responseClientProfile.ClientRegistrationResults =
                        new ClientProfile.BatchCreateClientsResponseTypeClientRegistrationResult[0];
                }

                this.Logger.InfoFormat(
                    "Cервис ClientProfile при регистрации клиентов вернул успешный статус, количество клиентов в ответе сервиса: ({0})", 
                    responseClientProfile.ClientRegistrationResults.Count());

                responseClientProfile.ClientRegistrationResults.ToList().ForEach(
                    x =>
                    {
                        if (x.StatusCode != 0)
                        {
                            const string Mess =
                                "Регистрация участника в ClientProfile ({0}) c ExternalId ({1}) завершилась с ошибкой ({2}), код ошибки ({3})";
                            this.Logger.ErrorFormat(Mess, x.ClientId, x.ClientExternalId, x.Error, x.StatusCode);

                            findTrouble = true;
                            var clientForRegistration = responseList.Single(t => t.RegRequest.ClientId == x.ClientId);
                            clientForRegistration.RegResponse.Status = RegClientStatuses.Error;
                        }
                        else
                        {
                            const string Mess =
                                "Регистрация участника в ClientProfile ({0}) c ExternalId ({1}) успешно завершена";
                            this.Logger.InfoFormat(Mess, x.ClientId, x.ClientExternalId);
                        }
                    });

                if (findTrouble)
                {
                    this.Uow.Save();
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Ошибка при вызове ClientProfileService", ex);
                throw;
            }

            return responseClientProfile;
        }

        private ClientProfile.BatchCreateClientsRequestType PrepareClientProfileRequest(
            List<ClientRegistrationJoin> responseList)
        {
            var clientProfileRequestList = new List<ClientProfile.BatchCreateClientsRequestTypeClientRegistrationFact>();
            var clientProfileRequest = new ClientProfile.BatchCreateClientsRequestType
                                       {
                                           EtlSessionId = this.Logger.EtlSessionId, 
                                           LoyaltyProgramId = this.loyaltyProgramId, 
                                       };

            this.Logger.InfoFormat(
                "Подготовка запроса к сервису ClientProfile. Записей для обработки в текущей итерации: ({0})", 
                responseList.Count);

            responseList.ForEach(
                response =>
                {
                    response.RegRequest.ProfileClientId = response.RegRequest.ClientId;
                    var fact = response.RegRequest.ToBatchCreateClientsRequestTypeClientRegistrationFact();
                    clientProfileRequestList.Add(fact);
                });

            clientProfileRequest.ClientRegistrationFacts = clientProfileRequestList.ToArray();

            this.Logger.Info("Подготовка запроса к ClientProfile завершена, сохранение данных о выборке в БД");

            this.Uow.Save();

            this.Logger.Info("Сохранение изменений в БД завершено");

            return clientProfileRequest;
        }

        private void DeleteClientProfileRequest()
        {
            var clientsForRegistration =
                (from cl in
                    this.Uow.ClientForRegistrationResponseRepository.GetBySessionId(
                        Guid.Parse(this.Logger.EtlSessionId))
                 join fe in this.Uow.ClientForRegistrationRepository.GetAll() on cl.ClientId equals fe.ClientId
                 select new
                        {
                            fe
                        }).ToList();

            clientsForRegistration.ForEach(c => c.fe.IsDeleted = true);

            this.Uow.Save();
        }
    }
}