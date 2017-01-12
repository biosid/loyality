namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Data.SqlTypes;
    using System.Linq;
        
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;

    internal class PersonalMessageProcessor : ProcessorBase
    {
        private readonly IClientMessageService clientMessageService;
        private readonly ClientProfileService clientProfileService;

        public PersonalMessageProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IClientMessageService service,
            ClientProfileService clientProfileService)
            : base(logger, uow)
        {
            this.clientMessageService = service;
            this.clientProfileService = clientProfileService;
        }

        public void Process()
        {
            ClientPersonalMessage[] batch;

            int processedRecords = 0;

            var sessionId = Guid.Parse(this.Logger.EtlSessionId);

            //// skip null clients
            while ((batch = this.Uow.ClientPersonalMessageRepository.GetBySessionId(sessionId)
                               .Where(t => t.ClientId != null)
                               .OrderBy(x => x.ClientId)
                               .Skip(processedRecords)
                               .Take(ConfigHelper.BatchSize)
                               .ToArray()).Length > 0)
            {
                var dtoBatch = batch.Select(t => new Notification
                {
                    ClientId = t.ClientId,
                    Title = string.IsNullOrEmpty(t.Title) ? "Сообщение" : t.Title,
                    Text = t.Message,
                    ShowSince = t.FromDateTime,
                    ShowUntil = t.ToDateTime
                }).ToArray();

                var clientsIdentifiers =
                    dtoBatch.Select(x => x.ClientId)
                            .Distinct()
                            .Select(
                                x => new BatchGetClientsByExternalIdRequestTypeReqClientIdentifier { ClientExternalId = x })
                            .ToArray();
                var requestType = new BatchGetClientsByExternalIdRequestType
                                      {
                                          LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
                                          ReqClientsIdentifiers = clientsIdentifiers
                                      };
                var getClientRequest = new BatchGetClientsByExternalIdRequest { Request = requestType };

                Logger.InfoFormat(
                "Обращение к сервису ClientProfile для выбора только существующих участников: количество идентификаторов для проверки {0}",
                getClientRequest.Request.ReqClientsIdentifiers.Count());
                BatchGetClientsByExternalIdResponseType clientProfileResponse;
                try
                {
                    clientProfileResponse = clientProfileService.BatchGetClientsByExternalId(getClientRequest).Response;
                }
                catch (Exception ex)
                {
                    Logger.Error("Ошибка проверки профиля пользователя. ClientProfile", ex);
                    this.SaveMessagesForClientsWithoutProfile(dtoBatch.Select(x => new Tuple<string, int>(x.ClientId, PersonalMessageStatuses.ClientBlocked)), sessionId);
                    processedRecords += batch.Length;
                    continue;
                }

                if (clientProfileResponse.StatusCode != 0)
                {
                    Logger.ErrorFormat("Ошибка проверки профиля пользователей. ClientProfile. StatusCode: {0}", clientProfileResponse.StatusCode);
                    this.SaveMessagesForClientsWithoutProfile(dtoBatch.Select(t => new Tuple<string, int>(t.ClientId, PersonalMessageStatuses.ClientBlocked)), sessionId);
                    processedRecords += batch.Length;
                    continue;
                }

                Logger.InfoFormat("Записей в ответе ClientProfile: {0}", clientProfileResponse.ResClientsIdentifiers.Count());

                var idsWithProfile = clientProfileResponse.ResClientsIdentifiers.Where(t => t.StatusCode == 0).Select(t => t.ClientId);

                //// we can receive ClientId = null and ClientExternalId = "blabla" in this cast we use external Id
                var idsWithoutProfile = clientProfileResponse.ResClientsIdentifiers
                    .Where(t => t.StatusCode != 0)
                    .Select(t => new Tuple<string, int>(t.ClientId ?? t.ClientExternalId, PersonalMessageStatuses.ClientNotFound))
                    .ToList();

                //// persist messages for clients with profile
                var messagesWithProfile = dtoBatch.Where(t => idsWithProfile.Contains(t.ClientId)).ToArray();
                Logger.InfoFormat("Сохранить {0} сообщений для клиентов с профилем в системе Нотификаций", messagesWithProfile.Count());
                
                NotifyClientsResult notifyClientsResult;
                
                try
                {
                    var parameters = new NotifyClientsParameters()
                                     {
                                         Notifications = messagesWithProfile
                                     };

                    notifyClientsResult = this.clientMessageService.Notify(parameters);
                }
                catch (Exception ex)
                {
                    Logger.Error("Ошибка сохраниения списка сообщений. ClientMessageService", ex);
                    this.SaveMessagesForClientsWithoutProfile(idsWithoutProfile, sessionId);
                    processedRecords += batch.Length;
                    continue;
                }

                if (notifyClientsResult.ResultCode != 0)
                {
                    Logger.ErrorFormat("Ошибка сохраниения списка сообщений. ClientInboxService. ResultCode: {0}", notifyClientsResult.ResultCode);
                    this.SaveMessagesForClientsWithoutProfile(idsWithoutProfile, sessionId);
                    processedRecords += batch.Length;
                    continue;
                }

                Logger.InfoFormat("Сохранить {0} сообщений для клиентов с профилем в системе Лояльность", notifyClientsResult.Threads.Count());
                //// persist messages for clients with profile
                foreach (var r in notifyClientsResult.Threads)
                {
                    this.Uow.ClientPersonalMessageResponseRepository.Add(new ClientPersonalMessageResponse
                            {
                                ClientId = r.ClientId,
                                SessionId = sessionId,
                                Status = PersonalMessageStatuses.Success
                            });
                }

                try
                {
                    this.Uow.Save();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Logger.ErrorFormat(
                                "DbEntityValidationException Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

                //// persist messages for clients without profile
                Logger.InfoFormat("Сохранить {0} сообщений для клиентов без профиля в системе Лояльность", idsWithoutProfile.Count());
                this.SaveMessagesForClientsWithoutProfile(idsWithoutProfile, sessionId);

                processedRecords += batch.Length;
                this.Logger.InfoFormat("Обработано записей ({0})", processedRecords);
            }
        }

        private void SaveMessagesForClientsWithoutProfile(IEnumerable<Tuple<string, int>> idsWithoutProfile, Guid sessionId)
        {
            foreach (var t in idsWithoutProfile)
            {
                this.Logger.InfoFormat("Нет профиля для клиента c ClientId (ClientExternalId): {0}", t.Item1);
                var temp = new ClientPersonalMessageResponse { ClientId = t.Item1, SessionId = sessionId, Status = t.Item2 };
                this.Uow.ClientPersonalMessageResponseRepository.Add(temp);
            }

            try
            {
                this.Uow.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.ErrorFormat(
                            "DbEntityValidationException Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}