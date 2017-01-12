using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using ProductCatalog.WsClients.OrderManagementService;

    /// <summary>
    /// Интерфейс взаимодейстивия с клиентскими каталогом подарков.
    /// </summary>
    public interface IProductCatalogProvider
    {
        ChangeExternalOrdersStatusesResult ChangeOrdersStatuses(ExternalOrdersStatus[] ordersStatus);

        CreateOrderResult CreateOrderForOnlinePartner(CreateOrderFromOnlinePartnerParameters parameters);

        GetOrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters);
    }
}
