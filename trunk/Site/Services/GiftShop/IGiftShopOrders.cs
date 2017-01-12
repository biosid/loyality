using System;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop
{
    public interface IGiftShopOrders
    {
        GiftShopOrder CreateOrderDraft(string clientId, MechanicsContext attributes, Guid[] basketItemIds, OrderDeliveryParameters deliveryParameters, decimal totalAdvance);

        GiftShopOrder ConfirmOrder(string clientId, int orderId);

        DeliveryAddressesResult GetDeliveryAddresses(string clientId, bool excludeAddressesWithoutKladr);

        DeliveryVariants GetDeliveryVariants(string clientId, MechanicsContext attributes, Guid[] basketItemId, DeliveryLocationInfo locationInfo);

        GiftShopOrdersResult GetOrdersHistory(string clientId, OrderStatus[] statuses, DateTimeRange range, PagingSettings paging);

        GiftShopOrder GetOrder(string clientId, int orderId);

        GiftShopOrder GetOrderByExternalId(string clientId, int partnerId, string externalOrderId);

        GiftShopOrder CreateCustomOrder(string clientId, int partnerId, string externalOrderId, string articleId, string articleName, decimal priceBonus, decimal priceRur);

        void ChangeOrderStatus(string clientId, int orderId, OrderStatus status);
    }
}
