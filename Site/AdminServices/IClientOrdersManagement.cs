using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Arms.AdminServices
{
    public interface IClientOrdersManagement
    {
        GiftShopOrdersResult GetOrders(string clientId, OrderStatus[] statuses, DateTimeRange range, PagingSettings paging);

        GiftShopOrder GetOrder(string clientId, int orderId);
    }
}
