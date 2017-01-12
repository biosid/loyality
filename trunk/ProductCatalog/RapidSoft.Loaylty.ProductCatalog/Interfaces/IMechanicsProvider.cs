namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using System.Collections.Generic;

    using API.Entities;

    using PromoAction.WsClients.MechanicsService;

    using Services;

    public interface IMechanicsProvider
    {
        GenerateResult GetPriceSql(Dictionary<string, string> clientContext);

        CalculateResult CalculateProductPrice(Dictionary<string, string> clientContext, decimal priceRUR, Product product);

        CalculatedPrice CalculateDeliveryPrice(Dictionary<string, string> clientContext, decimal deliveryPrice, int partnerId);

        CalculatedPrice[] CalculateDeliveryPrices(Dictionary<string, string> clientContext, decimal[] deliveryPrices, int partnerId);

        FactorsResult GetOnlineProductFactors(Dictionary<string, string> clientContext);
    }
}
