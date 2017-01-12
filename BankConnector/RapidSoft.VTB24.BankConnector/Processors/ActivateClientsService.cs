namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Extension;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Processors.MapHelpers;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    using ClientProfile = RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using Processing = RapidSoft.Loaylty.Processing.ProcessingService;

    internal class ActivateClientsService : ProcessorBase
    {
        private readonly ISecurityWebApi securityWebApi;

        private readonly ClientProfile.ClientProfileService clientProfileServicePort;

        private readonly Processing.ProcessingService processingServicePort;

        private readonly ITargetAudienceService targetAudienceService;

        public ActivateClientsService(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            Processing.ProcessingService processingServicePort,
            ClientProfile.ClientProfileService clientProfileServicePort,
            ITargetAudienceService targetAudienceService,
            ISecurityWebApi securityWebApi)
            : base(logger, uow)
        {
            this.processingServicePort = processingServicePort;
            this.clientProfileServicePort = clientProfileServicePort;
            this.targetAudienceService = targetAudienceService;
            this.securityWebApi = securityWebApi;
        }

        public void Execute()
        {
            ClientForActivation[] batch;
            var processedRecords = 0;
            var sessionId = Guid.Parse(this.Logger.EtlSessionId);

            // NOTE: VTBPLK-1743: Помечаем записи которые не будем отправлять, так как clientId есть в [dbo].[ClientForDeletion]
            this.SetDeletionEtlSessionId(sessionId);

            var targetEntities = this.Uow.ClientForActivationRepository.GetUniqueClientIdBySession(sessionId);

            this.Logger.InfoFormat("Старт пакетной обработки записей ({0} записей в группе)", ConfigHelper.BatchSize);
            while (
                (batch =
                    targetEntities.OrderBy(x => x.ClientId)
                                  .Skip(processedRecords)
                                  .Take(ConfigHelper.BatchSize)
                                  .ToArray()).Length > 0)
            {
                // Проверяем что клиенты зарегистрированы
                var registredClients = GetSecurityExistedClients(batch);
                this.Logger.InfoFormat("Успешно найденных в безопасности: {0}", registredClients.Length);

                var profileUpdateSuccess = this.CallProfileUpdate(registredClients);
                this.Logger.InfoFormat("Успешно обновленных записей профилей: {0}", profileUpdateSuccess.Length);

                var profileActivateSuccess = this.CallProfileActivate(profileUpdateSuccess);                
                this.Logger.InfoFormat("Успешно активированных записей профилей: {0}", profileActivateSuccess.Length);                
                
                var succeeded = this.CallProcessing(profileActivateSuccess);

                foreach (var client in succeeded)
                {
                    client.Status = (int)ActivateClientStatus.Success;
                    this.Uow.ClientForActivationRepository.Update(client);
                    this.Logger.InfoFormat("Активация клиента ({0}) выполнена успешно", client.ClientId);
                }

                this.Logger.InfoFormat(
                    "Завершение активации профиля в сервисе процессинга Count = {0}", succeeded.Length);

                this.Logger.Info("Формируем набор сегментов с идентификаторами клиента для привязки в компоненте \"Акции\" и регистрируем");
                var segments = succeeded.GroupBy(x => x.Segment).Select(group => group.ToSegment(x => x.ClientId));

                try
                {
                    this.targetAudienceService.CallAssignClientSegment(segments, this.Logger);
                    this.Logger.Info("Регистрация в сегментах выполнена успешно");
                }
                catch (Exception ex)
                {
                    this.Logger.Error("ошибка регистрации клиентов в сегментах", ex);
                }
                
                this.Uow.Save();

                processedRecords += batch.Length;
                this.Logger.InfoFormat("Обработано записей ({0})", processedRecords);
            }

            this.UpdateOldClientRecords();
        }

        private ClientForActivation[] GetSecurityExistedClients(ClientForActivation[] clients)
        {
            if (clients.Length == 0)
            {
                return new ClientForActivation[0];
            }

            try
            {
                var resolveUsersResp = securityWebApi.BatchResolveUsersByClientId(clients.Select(x => x.ClientId).ToArray());

                if (resolveUsersResp == null)
                {
                    throw new Exception("SecurityWebApi.BatchResolveUsersByPhone response returns with null");
                }

                var existedClients = (from r in resolveUsersResp
                                      join c in clients on r.Key equals c.ClientId
                                      where
                                          r.Value != null
                                          &&
                                          r.Value.ClientId == c.ClientId
                                      select c).ToArray();

                var notExistedClients = clients.Where(c => !existedClients.Contains(c)).ToArray();

                foreach (var client in notExistedClients)
                {
                    Logger.Warn(string.Format("Клиент не найдён в Security {0}", client.ClientId));
                    client.Status = (int)ActivateClientStatus.WrongClientId;
                }

                return existedClients.ToArray();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при проверке существования клиентов в Security", ex);
                MarkAllFailed(clients);
                return new ClientForActivation[0];
            }
        }

        /// <summary>
        ///     VTBPLK-1743: Метод устанавливает DeletionEtlSessionId для клиентов которые уже есть в списке на удаление ([dbo].[ClientForDeletion])
        /// </summary>
        /// <param name="sessionId">Идентификатор текущей etl сессии</param>
        private void SetDeletionEtlSessionId(Guid sessionId)
        {
            var forDeletion = this.Uow.ClientForDeletionRepository.GetAll();
            var forActivation = this.Uow.ClientForActivationRepository.GetAll().Where(x => x.EtlSessionId == sessionId);
            var joined = forActivation.Join(
                forDeletion, 
                activation => activation.ClientId, 
                deletion => deletion.ExternalClientId, 
                (activation, deletion) => new
                                          {
                                              activation, 
                                              deletion.InsertEtlSessionId
                                          });
            joined.ToList().ForEach(
                x =>
                {
                    x.activation.DeletionEtlSessionId = x.InsertEtlSessionId;
                    x.activation.Status = (int)ActivateClientStatus.ClientForDeletion;
                });
            this.Uow.Save();
        }

        private ClientForActivation[] CallProcessing(ClientForActivation[] batch)
        {
            if (batch.Length == 0)
            {
                return new ClientForActivation[] { };
            }

            var request = new Processing.BatchActivateClientsRequest
                          {
                              Request =
                                  new Processing.BatchActivateClientsRequestType
                                  {
                                      EtlSessionId = this.Logger.EtlSessionId,
                                      LoyaltyProgramId = ConfigHelper.LoyaltyProgramId
                                  }
                          };

            request.Request.ActivationFacts =
                batch.Select(
                    c => new Processing.BatchActivateClientsRequestTypeActivationFact { ClientExternalId = c.ClientId })
                     .ToArray();

            Processing.BatchActivateClientsResponseType response;

            this.Logger.InfoFormat(
                "Попытка вызова Processing, число записей в запросе: ({0})", request.Request.ActivationFacts.Count());
            try
            {
                response = this.processingServicePort.BatchActivateClients(request).Response;
            }
            catch (Exception e)
            {
                this.Logger.Error("Ошибка при вызове сервиса процессинга", e);
                MarkAllFailed(batch);
                return new ClientForActivation[0];
            }

            if (response.StatusCode.IsFail())
            {
                this.Logger.Error(string.Format("Ошибка вызова сервиса процессинга Status = {0}", response.StatusCode));
                MarkAllFailed(batch);
                return new ClientForActivation[0];
            }

            var map = batch.Zip(response.ClientActivationResults, Tuple.Create).ToArray();

            // TODO: по ТЗ возвращен должен быть одит из 2 ошибочных статусов: не найден Id клиента или клиент уже был активирован ранее
            // требуется уточнить значения статусов, предоставляемых в ответе процессинга
            var failed = map.Where(r => r.Item2.StatusCode.IsFail()).ToArray();

            if (failed.Any())
            {
                this.Logger.ErrorFormat("Ошибка активации профиля в сервисе процессинга Count = {0}", failed.Length);

                foreach (var pair in failed)
                {
                    var client = pair.Item1;
                    var result = pair.Item2;

                    client.Status = (int)ActivateClientStatus.WrongClientId;
                    client.Reason = result.Error;

                    this.Logger.ErrorFormat(
                        "При активации в Processing клиента ({0}) c ExternalId ({1}) произошла ошибка ({2}), описание ошибки ({3})",
                        result.ClientId,
                        result.ClientExternalId,
                        result.StatusCode,
                        result.Error);
                }
            }

            var succeeded = map.Where(r => !r.Item2.StatusCode.IsFail()).Select(r => r.Item1).ToArray();

            return succeeded;
        }

        private ClientForActivation[] CallProfileActivate(ClientForActivation[] batch)
        {
            if (batch.Length == 0)
            {
                return new ClientForActivation[] { };
            }

            var activationFacts = batch.Select(c => c.ToBatchActivateClientsRequestTypeClientActivationFact()).ToArray();

            var requestType = new ClientProfile.BatchActivateClientsRequestType
                              {
                                  EtlSessionId = this.Logger.EtlSessionId, 
                                  LoyaltyProgramId = ConfigHelper.LoyaltyProgramId, 
                                  ClientActivationFacts = activationFacts, 
                              };

            var request = new ClientProfile.BatchActivateClientsRequest
                          {
                              Request = requestType
                          };

            ClientProfile.BatchActivateClientsResponseType response;

            this.Logger.InfoFormat(
                "Попытка вызова ClientProfile, число записей в запросе: ({0})", 
                request.Request.ClientActivationFacts.Count());
            try
            {
                response = this.clientProfileServicePort.BatchActivateClients(request).Response;
            }
            catch (Exception e)
            {
                this.Logger.Error("Ошибка при вызове сервиса ClientProfile", e);
                MarkAllFailed(batch);
                return new ClientForActivation[0];
            }

            if (response.StatusCode.IsFail())
            {
                this.Logger.Error(string.Format("Запрос ClientProfile вернул ошибочный статус Status = {0}", response.StatusCode));
                MarkAllFailed(batch);
                return new ClientForActivation[0];
            }

            var map = batch.Zip(response.ClientActivationResults, Tuple.Create).ToArray();

            // TODO: по ТЗ возвращен должен быть одит из 2 ошибочных статусов: не найден Id клиента или клиент уже был активирован ранее
            // требуется уточнить значения статусов, предоставляемых в ответе процессинга
            var fail = map.Where(r => r.Item2.StatusCode.IsFail()).ToList();

            if (fail.Any())
            {
                this.Logger.ErrorFormat("Ошибка активации профиля в сервисе профиля Count = {0}", fail.Count());
                foreach (var pair in fail)
                {
                    var client = pair.Item1;
                    var result = pair.Item2;

                    client.Status = (int)ActivateClientStatus.WrongClientId;
                    client.Reason = result.Error;

                    this.Logger.ErrorFormat(
                        "При активации в ClientProfile клиента ({0}) c ExternalId ({1}) произошла ошибка ({2}), описание ошибки ({3})",
                        result.ClientId,
                        result.ClientExternalId,
                        result.StatusCode,
                        result.Error);
                }
            }

            var success = map.Where(r => !r.Item2.StatusCode.IsFail()).Select(r => r.Item1).ToArray();

            this.Logger.InfoFormat("Завершение активации профиля в сервисе профиля Count = {0}", success.Count());

            return success;
        }

        private ClientForActivation[] CallProfileUpdate(IEnumerable<ClientForActivation> batch)
        {          
            var successList = new List<ClientForActivation>();

            foreach (var forActivation in batch)
            {
                var request = BuildClientProfileUpdateRequest(forActivation);

                ClientProfile.UpdateClientProfileResponse response;

                try
                {
                    response =
                        this.clientProfileServicePort.UpdateClientProfile(
                            new ClientProfile.UpdateClientProfileRequest(
                                new ClientProfile.UpdateClientProfileRequestType
                                {
                                    ClientProfile = request
                                }));
                }
                catch (Exception ex)
                {
                    var description = string.Format(
                        "ошибка при вызове сервиса ClientProfile.UpdateClientProfile для клиента ({0}): {1}", forActivation.ClientId, ex);
                    this.Logger.ErrorFormat(description);
                    forActivation.Status = (int)ActivateClientStatus.Error;
                    continue;
                }

                if (response == null || response.Response == null)
                {
                    var description =
                        string.Format(
                            "Сервис ClientProfile не вернул ответа на запрос обновления профиля клиента ({0})", 
                            forActivation.ClientId);
                    this.Logger.ErrorFormat(description);
                    forActivation.Status = (int)ActivateClientStatus.Error;
                    continue;
                }

                if (response.Response.StatusCode != 0)
                {
                    var description =
                        string.Format(
                            "Произошла ошибка при обновлении данных клиента ({0}), код ошибки - {1}, описание ошибки - {2}", 
                            forActivation.ClientId, 
                            response.Response.StatusCode, 
                            response.Response.Error);
                    this.Logger.ErrorFormat(description);
                    forActivation.Status = (int)ActivateClientStatus.WrongClientId;
                    continue;
                }

                successList.Add(forActivation);
            }

            return successList.ToArray();
        }

        private void MarkAllFailed(IEnumerable<ClientForActivation> batch)
        {
            foreach (var client in batch)
            {
                client.Status = (int)ActivateClientStatus.Error;
            }
        }

        private ClientProfile.UpdateClientProfileRequestTypeClientProfile BuildClientProfileUpdateRequest(ClientForActivation forActivation)
        {
            return new ClientProfile.UpdateClientProfileRequestTypeClientProfile
            {
                ClientId = forActivation.ClientId,
                BirthDate = BuildDateTimeUpdateField(forActivation.BirthDate),
                Email = BuildStringUpdateField(forActivation.Email),
                Gender = BuildStringUpdateField(forActivation.Gender.ToString(CultureInfo.InvariantCulture)),
                FirstName = BuildStringUpdateField(forActivation.FirstName),
                MiddleName = BuildStringUpdateField(forActivation.MiddleName),
                LastName = BuildStringUpdateField(forActivation.LastName)
            };
        }

        private ClientProfile.ElementDateTimeWithAttribute BuildDateTimeUpdateField(DateTime value)
        {
            return new ClientProfile.ElementDateTimeWithAttribute
            {
                actionSpecified = true,
                action = ClientProfile.Action.U,
                Value = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };
        }

        private ClientProfile.ElementStringWithAttribute BuildStringUpdateField(string value)
        {
            return new ClientProfile.ElementStringWithAttribute
            {
                actionSpecified = true,
                action = ClientProfile.Action.U,
                Value = value
            };
        }

        private void UpdateOldClientRecords()
        {
            try
            {
                var repository = this.Uow.ClientForActivationRepository;

                var sessionIdGuid = Guid.Parse(this.Logger.EtlSessionId);
                var duplicates = (from c in repository.GetAll()
                                  join s in repository.GetOtherEtlSessionsClientRecords(sessionIdGuid) on c.ClientId
                                      equals s.ClientId into lj
                                  from j in lj.DefaultIfEmpty()
                                  where c.EtlSessionId == sessionIdGuid && j.EtlSessionId != null
                                  select c).ToList();
                duplicates.ForEach(
                    x =>
                    {
                        x.Status = (int)ActivateClientStatus.AlreadyActivated;
                        x.Reason = string.Format("Client with ClientId = ({0}) already activated", x.ClientId);
                        repository.Update(x);
                    });
                this.Uow.Save();
            }
            catch (Exception e)
            {
                this.Logger.Error("Ошибка при обновлениии статуса для ранее активированных клиентов", e);
            }
        }
    }
}