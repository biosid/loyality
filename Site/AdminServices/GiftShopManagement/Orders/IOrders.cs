using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    public interface IOrders
    {
        Order GetOrder(int id);

        OrdersSearchResult SearchOrders(OrdersSearchCriteria criteria, PagingSettings paging);

        void ChangeOrderStatus(ChangeOrderStatusOptions options);

        OrderStatusHistoryRecord[] GetOrderStatusHistory(int id);
    }
}