namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Processors.MapHelpers;
    using RapidSoft.VTB24.BankConnector.Service;

    public class ClientUpdater : ProcessorBase
    {
        private readonly ClientProfileService clientProfileServicePort;

        private readonly ITargetAudienceService targetAudienceService;

        public ClientUpdater(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            ClientProfileService clientProfileServicePort,
            ITargetAudienceService targetAudienceService)
            : base(logger, uow)
        {
            this.clientProfileServicePort = clientProfileServicePort;
            this.targetAudienceService = targetAudienceService;
        }

        public void UpdateClients()
        {
            Logger.InfoFormat("Обновление клиентов");

            var sessionId = new Guid(Logger.EtlSessionId);
            int totalProcessed = 0;
            int batchCount = 0;

            do
            {
                var clientUpdates =
                    this.Uow.ClientUpdatesRepository.GetBySessionId(sessionId)
                        .OrderBy(c => c.SeqId)
                        .Skip(totalProcessed)
                        .Take(ConfigHelper.BatchSize)
                        .ToArray();

                this.Logger.Info("Обновление профилей клиентов");

                foreach (var update in clientUpdates)
                {
                    try
                    {
                        if (update.Segment.HasValue && (update.Segment.Value != 0 && update.Segment.Value != 1))
                        {
                            update.UpdStatus = ClientUpdateStatuses.WrongSegment;
                            continue;
                        }

                        if (update.Gender.HasValue && (update.Gender.Value < 0 || update.Gender.Value > 2))
                        {
                            update.UpdStatus = ClientUpdateStatuses.WrongGender;
                            continue;
                        }

                        var currentProfile = this.GetCurrentProfile(update);

                        if (currentProfile == null)
                        {
                            continue;
                        }

                        if (currentProfile.ClientStatus == (int)ClientProfileClientStatusCodes.Created)
                        {
                            update.UpdStatus = ClientUpdateStatuses.NotActivated;
                            continue;
                        }

                        this.ClientProfileUpdateClient(currentProfile, update);
                    }
                    catch (Exception e)
                    {
                        Logger.ErrorFormat(e, "Не удалось обновить профиль клиента {0}", update.ClientId);
                        update.UpdStatus = ClientUpdateStatuses.Error;
                    }
                }

                this.Logger.Info("Обновление профилей клиентов закончено");

                var validUpdates = clientUpdates.Where(c => !c.UpdStatus.HasValue).ToArray();

                try
                {
                    this.Logger.Info("Регистрация клиентов в сегментах");

                    var segments =
                        validUpdates.GroupBy(x => x.Segment).Select(group => group.ToSegment(x => x.ClientId)).ToArray();
                    this.targetAudienceService.CallAssignClientSegment(segments, this.Logger);

                    validUpdates.ToList().ForEach(c => c.UpdStatus = ClientUpdateStatuses.Success);
                    this.Logger.Info("Регистрация клиентов в сегментах выполнена успешно");
                }
                catch (Exception e)
                {
                    validUpdates.ToList().ForEach(c => c.UpdStatus = ClientUpdateStatuses.Error);
                    Console.WriteLine(e);
                }

                clientUpdates.ToList().ForEach(
                    c =>
                    {
                        c.FirstName = ProcessConstants.NotSpecified;
                        c.LastName = ProcessConstants.NotSpecified;
                        c.MiddleName = ProcessConstants.NotSpecified;
                        c.Email = ProcessConstants.NotSpecified;
                        c.BirthDate = DateTime.MinValue;
                    });

                Uow.Save();

                batchCount = clientUpdates.Length;
                totalProcessed += batchCount;
                this.Logger.Info("Total processed - " + totalProcessed);
            }
            while (batchCount > 0);
            
            Logger.InfoFormat("Обновление клиентов выполнено");
        }

        private bool ClientProfileUpdateClient(
            GetClientProfileFullResponseTypeClientProfile currentProfile, ClientUpdate update)
        {
            var request = this.BuildClientProfileUpdateRequest(currentProfile, update);

            var updateResponse =
                this.clientProfileServicePort.UpdateClientProfile(
                    new UpdateClientProfileRequest(
                        new UpdateClientProfileRequestType
                        {
                            ClientProfile = request
                        }));

            if (updateResponse == null)
            {
                var message = string.Format(
                    "clId:{0} clientProfile.UpdateClientProfile updateResponse is null", update.ClientId);
                this.Logger.ErrorFormat(message);
                throw new Exception(message);
            }

            if (updateResponse.Response == null)
            {
                var message = string.Format(
                    "clId:{0} clientProfile.UpdateClientProfile updateResponse.Response is null", update.ClientId);
                this.Logger.ErrorFormat(message);
                throw new Exception(message);
            }

            if (updateResponse.Response.StatusCode == 1)
            {
                update.UpdStatus = ClientUpdateStatuses.Error;
                var message = string.Format(
                    "clId:{0} clientProfile.UpdateClientProfile error StatusCode:{1}", update.ClientId, updateResponse.Response.StatusCode);
                this.Logger.ErrorFormat(message);
                return false;
            }

            if (updateResponse.Response.StatusCode == 2)
            {
                update.UpdStatus = ClientUpdateStatuses.ClientNotFound;
                var message = string.Format(
                    "clId:{0} clientProfile.UpdateClientProfile ClientNotFound StatusCode:{1}", update.ClientId, updateResponse.Response.StatusCode);
                this.Logger.ErrorFormat(message);
                return false;
            }

            if (updateResponse.Response.StatusCode < 0 || updateResponse.Response.StatusCode > 2)
            {
                update.UpdStatus = ClientUpdateStatuses.Error;
                var message = string.Format(
                    "clId:{0} clientProfile.UpdateClientProfile wrong StatusCode:{1}", update.ClientId, updateResponse.Response.StatusCode);
                this.Logger.ErrorFormat(message);
                return false;
            }

            return true;
        }

        private GetClientProfileFullResponseTypeClientProfile GetCurrentProfile(ClientUpdate update)
        {
            var getProfileResponse = this.clientProfileServicePort.GetClientProfileFull(
                new GetClientProfileFullRequest
                {
                    Request = new GetClientProfileFullRequestType
                              {
                                  ClientId = update.ClientId
                              }
                });

            if (getProfileResponse == null)
            {
                var message = string.Format(
                    "clId:{0} clientProfile.GetClientProfileFull getProfileResponse is null", update.ClientId);
                this.Logger.ErrorFormat(message);
                throw new Exception(message);
            }

            if (getProfileResponse.Response == null)
            {
                var message = string.Format(
                    "clId:{0} clientProfile.GetClientProfileFull getProfileResponse.Response is null", update.ClientId);
                this.Logger.ErrorFormat(message);
                throw new Exception(message);
            }

            if (getProfileResponse.Response.StatusCode == 1 || (getProfileResponse.Response.StatusCode < 0 || getProfileResponse.Response.StatusCode > 2))
            {
                update.UpdStatus = ClientUpdateStatuses.Error;
                this.Logger.ErrorFormat(
                    string.Format(
                        "clId:{0} clientProfile.GetClientProfileFull unknown StatusCode:{1}",
                        update.ClientId,
                        getProfileResponse.Response.StatusCode));
                return null;
            }

            if (getProfileResponse.Response.StatusCode == 2)
            {
                update.UpdStatus = ClientUpdateStatuses.ClientNotFound;
                this.Logger.ErrorFormat(
                    string.Format(
                        "clId:{0} clientProfile.GetClientProfileFull StatusCode:{1}",
                        update.ClientId,
                        getProfileResponse.Response.StatusCode));
                return null;
            }

            if (getProfileResponse.Response.ClientProfile == null)
            {
                var message =
                    string.Format(
                        "clId:{0} clientProfile.GetClientProfileFull getProfileResponse.Response.ClientProfile is null",
                        update.ClientId);
                this.Logger.ErrorFormat(message);
                throw new Exception(message);
            }

            return getProfileResponse.Response.ClientProfile;
        }

        private UpdateClientProfileRequestTypeClientProfile BuildClientProfileUpdateRequest(GetClientProfileFullResponseTypeClientProfile currentProfile, ClientUpdate update)
        {
            Func<string, ElementStringWithAttribute> buildStringUpdateField =
                x =>
                new ElementStringWithAttribute
                    {
                        action = Loaylty.ClientProfile.ClientProfileService.Action.U,
                        actionSpecified = true,
                        Value = x,
                    };

            var request = new UpdateClientProfileRequestTypeClientProfile();

            if (update.BirthDate.HasValue)
            {
                request.BirthDate = new ElementDateTimeWithAttribute
                {
                    actionSpecified = true,
                    action = Loaylty.ClientProfile.ClientProfileService.Action.U,
                    Value = update.BirthDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                };
            }

            if (update.Gender.HasValue)
            {
                request.Gender = buildStringUpdateField(update.Gender.Value.ToString());
            }

            request.ClientId = update.ClientId;

            if (request.Email != null)
            {
                request.Email = buildStringUpdateField(update.Email);
            }

            request.FirstName = buildStringUpdateField(update.FirstName);

            if (request.MiddleName != null)
            {
                request.MiddleName = buildStringUpdateField(update.MiddleName);
            }

            request.LastName = buildStringUpdateField(update.LastName);

            return request;
        }
    }
}