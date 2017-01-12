namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    using Entities;

    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;

    public class GetPopularProductParameters
    {
        public PopularProductTypes? PopularProductType
        {
            get;
            set;
        }

        public int? CountToTake
        {
            get;
            set;
        }

        public Dictionary<string, string> ClientContext
        {
            get;
            set;
        }

        public SortTypes SortType
        {
            get;
            set;
        }

        public GenerateResult PriceSql = null;
    }
}
