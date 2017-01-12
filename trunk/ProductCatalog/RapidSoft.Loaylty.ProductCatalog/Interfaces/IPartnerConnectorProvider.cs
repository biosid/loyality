namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using GetDeliveryVariantsResult = PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult;
    using Location = API.InputParameters.Location;
    using Order = API.Entities.Order;
    using OrderItem = API.Entities.OrderItem;

    public interface IPartnerConnectorProvider
    {
        CheckOrderResult CheckOrder(Order order);

        CommitOrderResult CommitOrder(Order order);

        ResultBase CustomCommitOrder(Order order, string methodName);

        FixBasketItemPriceResult FixBasketItemPrice(FixBasketItemPriceParam param);

        GetDeliveryVariantsResult GetDeliveryVariants(Location location, OrderItem orderItem, string clientId, int partnerId);

        GetDeliveryVariantsResult GetDeliveryVariants(Location location, OrderItem[] orderItems, string clientId, int partnerId);
    }
}