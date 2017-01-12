namespace RapidSoft.VTB24.BankConnector.Extension
{
	using System;
	using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
	using RapidSoft.VTB24.BankConnector.DataModels;

    public static class RuleExtensions
    {
        public static PromoAction BuildPromoAction(
            this Rule rule, DateTime dateSent, int indexSent, string etlSessionId)
        {
            return new PromoAction
                   {
                       EtlSessionId = etlSessionId, 
                       PromoId = rule.Id, 
                       Description = rule.Name, 
                       FromDate = rule.DateTimeFrom, 
                       ToDate = rule.DateTimeTo, 
                       DateSent = dateSent, 
                       IndexSent = indexSent
                   };
        }
    }
}