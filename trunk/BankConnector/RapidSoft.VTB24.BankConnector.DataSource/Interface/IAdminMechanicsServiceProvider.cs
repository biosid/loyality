namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IAdminMechanicsServiceProvider
    {
        IList<Rule> GetPromoActions(DateTime dateTimeFrom, bool activeOnly = true, params ApproveStatus[] approveStatuses);

        void SetPromoActionsStatus(IList<PromoActionResponse> promoActionResponses);
    }
}
