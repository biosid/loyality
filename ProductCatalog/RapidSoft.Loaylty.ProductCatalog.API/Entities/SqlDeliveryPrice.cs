namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;

    public class SqlDeliveryPrice
    {
        public GenerateResult DeliveryPriceSql { get; set; }

        public GenerateResult DeliveryPriceQuantitySql { get; set; }
    }
}