namespace RapidSoft.VTB24.BankConnector.Processors.MapHelpers
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.DataModels;

    using ClientProfilBatchCreateClientsRequestTypeClientRegistrationFact = RapidSoft.Loaylty.ClientProfile.ClientProfileService.BatchCreateClientsRequestTypeClientRegistrationFact;
    using ClientProfileBatchActivateClientsRequestTypeClientActivationFact = 
RapidSoft.Loaylty.ClientProfile.ClientProfileService.BatchActivateClientsRequestTypeClientActivationFact;

    using ClientProfileBatchCreateClientsResponseTypeClientRegistrationResult =
RapidSoft.Loaylty.ClientProfile.ClientProfileService.BatchCreateClientsResponseTypeClientRegistrationResult;

    using ProcessingBatchCreateClientsRequestTypeClientRegistrationFact =
RapidSoft.Loaylty.Processing.ProcessingService.BatchCreateClientsRequestTypeClientRegistrationFact;

    public static class MapHelpers
    {
        public static ProcessingBatchCreateClientsRequestTypeClientRegistrationFact
            ToBatchCreateClientsRequestTypeClientRegistrationFact(
            this ClientProfileBatchCreateClientsResponseTypeClientRegistrationResult result)
        {
            var now = DateTime.Now;
            var retVal = new ProcessingBatchCreateClientsRequestTypeClientRegistrationFact
                             {
                                 RegistrationDateTime = now,
                                 ClientExternalId =
                                     result.ClientExternalId,
                                 ClientId = result.ClientId
                             };
            return retVal;
        }

        public static ClientProfilBatchCreateClientsRequestTypeClientRegistrationFact
            ToBatchCreateClientsRequestTypeClientRegistrationFact(
            this ClientForRegistration client)
        {
            var retVal = new ClientProfilBatchCreateClientsRequestTypeClientRegistrationFact
                             {
                                 ClientExternalId =
                                     client.ClientId,
                                 ClientId =
                                     client
                                     .ProfileClientId,
                                 FirstName =
                                     client.FirstName,
                                 LastName =
                                     client.LastName,
                                 MiddleName =
                                     client.MiddleName,
                                 Gender =
                                     (int)client.Gender,
                                 MobilePhone =
                                     client.MobilePhone,
                                 BirthDate =
                                     client.BirthDate
                             };
            return retVal;
        }

        public static ClientProfileBatchActivateClientsRequestTypeClientActivationFact
            ToBatchActivateClientsRequestTypeClientActivationFact(this ClientForActivation client)
        {
            return new ClientProfileBatchActivateClientsRequestTypeClientActivationFact
                       {
                           ClientExternalId =
                               client.ClientId
                       };
        }

        public static Segment ToSegment<T>(this IGrouping<int?, T> group, Func<T, string> getClientId)
        {
            var id = SegmentExtensions.GetSegmentId(group.Key.HasValue ? (ClientSegment?)group.Key.Value : null);
            var clientIds = group.Select(getClientId).ToArray();
            var segment = new Segment { Id = id, ClientIds = clientIds };
            return segment;
        }
    }
}
