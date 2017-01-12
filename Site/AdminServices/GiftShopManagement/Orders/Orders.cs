using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Outputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;
using Order = Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Order;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    public class Orders : IOrders
    {
        public Orders(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public Order GetOrder(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetOrderById(_security.CurrentUser, id);

                response.AssertSuccess();

                var order = MappingsFromService.ToOrder(response.Order);
                order.NextStatuses = response.NextOrderStatuses
                                             .Select(MappingsFromService.ToOrderStatus)
                                             .ToArray();

                return order;
            }
        }

        public OrdersSearchResult SearchOrders(OrdersSearchCriteria criteria, PagingSettings paging)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new SearchOrdersParameters
                {
                    UserId = _security.CurrentUser,

                    PartnerIds = criteria.SupplierIds,
                    CarrierIds = criteria.CarrierIds,
                    OrderIds = criteria.OrderId.HasValue ? new[] { criteria.OrderId.Value } : null,
                    StartDate = criteria.From,
                    EndDate = criteria.To,
                    Statuses = criteria.Statuses
                        .MaybeSelect(MappingsToService.ToOrderStatus)
                        .MaybeToArray(),
                    SkipStatuses = criteria.SkipStatuses
                        .MaybeSelect(MappingsToService.ToOrderStatus)
                        .MaybeToArray(),
                    OrderPaymentStatuses = criteria.ProductPaymentStatuses
                        .MaybeSelect(MappingsToService.ToOrderPaymentStatus)
                        .MaybeToArray(),
                    OrderDeliveryPaymentStatus = criteria.DeliveryPaymentStatuses
                        .MaybeSelect(MappingsToService.ToOrderDeliveryPaymentStatus)
                        .MaybeToArray(),

                    // пейджинг
                    CalcTotalCount = true,
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take
                };

                var response = service.SearchOrders(parameters);

                response.AssertSuccess();

                var orders = response.Orders.Select(MappingsFromService.ToOrder).ToArray();
                return new OrdersSearchResult(orders, response.TotalCount ?? orders.Count(), paging);
            }
        }

        public void ChangeOrderStatus(ChangeOrderStatusOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new[]
                {
                    new OrdersStatus
                    {
                        OrderId = options.Id,
                        OrderStatus = MappingsToService.ToOrderStatus(options.Status),
                        OrderStatusDescription = options.StatusDescription
                    }
                };

                var response = service.ChangeOrdersStatuses(_security.CurrentUser, parameters);

                response.AssertSuccess();
            }
        }

        public OrderStatusHistoryRecord[] GetOrderStatusHistory(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetOrderStatusesHistoryParameters
                {
                    UserId = _security.CurrentUser,
                    OrderId = id,
                    CountToSkip = 0,
                    CountToTake = 100
                };

                var response = service.GetOrderStatusesHistory(parameters);

                response.AssertSuccess();

                return response.OrderHistory.Select(MappingsFromService.ToOrderStatusHistoryRecord).ToArray();
            }
        }
    }
}