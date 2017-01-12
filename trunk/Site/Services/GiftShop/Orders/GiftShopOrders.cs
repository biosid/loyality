using System;
using System.Linq;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Orders.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.ProductCatalogOrderService;

namespace Vtb24.Site.Services.GiftShop.Orders
{
    public class GiftShopOrders : IGiftShopOrders, IDisposable
    {
        public const string ORDER_OTP_TYPE = "order_confirm";

        #region API

        public GiftShopOrder CreateOrderDraft(string clientId, MechanicsContext attributes, Guid[] basketItemIds, OrderDeliveryParameters delivery, decimal totalAdvance)
        {
            var parameters = new CreateOrderFromBasketItemsParameters
            {
                ClientId = clientId,
                ClientContext = attributes,
                BasketItems = basketItemIds,
                Delivery = MappingsToService.ToDeliveryDto(delivery),
                TotalAdvance = totalAdvance
            };

            using (var service = new OrderManagementServiceClient())
            {
                var response = service.CreateOrderFromBasketItems(parameters);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var order = MappingsFromService.ToGiftShopOrder(response.Order);

                return order;
            }
        }

        public GiftShopOrder ConfirmOrder(string clientId, int orderId)
        {
            var order = GetOrder(clientId, orderId);

            using (var service = new OrderManagementServiceClient())
            {
                var response = service.ClientCommitOrder(clientId, order.Id);

                AssertResponse(response.ResultCode, response.ResultDescription);

                return MappingsFromService.ToGiftShopOrder(response.Order);
            }
        }

        public DeliveryAddressesResult GetDeliveryAddresses(string clientId, bool excludeAddressesWithoutKladr)
        {
            // TODO: вынести в конфигурацию
            const int TAKE_COUNT = 10;

            using (var service = new OrderManagementServiceClient())
            {
                var response = service.GetLastDeliveryAddresses(clientId, excludeAddressesWithoutKladr, TAKE_COUNT);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var addresses = (response.Addresses ?? new ProductCatalogOrderService.LastDeliveryAddress[0])
                    .Where(a=>a != null)
                    .Select(MappingsFromService.ToLastDeliveryAddress)
                    .ToArray();


                return new DeliveryAddressesResult(
                    addresses, 
                    addresses.Length, 
                    PagingSettings.ByOffset(0, TAKE_COUNT)
                );
            }
        }

        public DeliveryVariants GetDeliveryVariants(string clientId, MechanicsContext attributes, Guid[] basketItemIds, DeliveryLocationInfo locationInfo)
        {
            var parameters = new GetDeliveryVariantsParameters
            {
                ClientId = clientId,
                ClientContext = attributes,
                BasketItems = basketItemIds,
                Location = MappingsToService.ToLocation(locationInfo)
            };

            using (var service = new OrderManagementServiceClient())
            {
                var response = service.GetDeliveryVariants(parameters);

                AssertResponse(response.ResultCode, response.ResultDescription);

                return new DeliveryVariants
                {
                    LocationName = response.Location != null ? response.Location.LocationName : null,
                    Groups = response.DeliveryGroups != null
                                 ? response.DeliveryGroups.Select(MappingsFromService.ToDeliveryVariantsGroup).ToArray()
                                 : new DeliveryVariantsGroup[0]
                };
            }
        }

        public GiftShopOrdersResult GetOrdersHistory(string clientId, OrderStatus[] statuses, DateTimeRange range, PagingSettings paging)
        {
            using (var service = new OrderManagementServiceClient())
            {
                var parameters = new GetOrdersHistoryParameters
                {
                    ClientId = clientId,
                    StartDate = range.Start,
                    EndDate = range.End,
                    CountToTake = paging.Take,
                    CountToSkip = paging.Skip,
                    CalcTotalCount = true,
                    Statuses = statuses.Select(MappingsToService.ToPublicOrderStatus).ToArray()
                };
                var response = service.GetOrdersHistory(parameters);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var orders = response.Orders.MaybeSelect(MappingsFromService.ToGiftShopOrder).MaybeToArray();
                return new GiftShopOrdersResult(orders, response.TotalCount != null ? response.TotalCount.Value : 0, paging);
            }
        }

        public GiftShopOrder GetOrder(string clientId, int orderId)
        {
            using (var service = new OrderManagementServiceClient())
            {
                var response = service.GetOrderById(orderId, clientId);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var order = MappingsFromService.ToGiftShopOrder(response.Order);
                return order;
            }
        }

        public GiftShopOrder GetOrderByExternalId(string clientId, int partnerId, string externalOrderId)
        {
            using (var service = new OrderManagementServiceClient())
            {
                var options = new GetOrderByExternalIdParameters
                {
                    ClientId = clientId,
                    PartnerId = partnerId,
                    ExternalOrderId = externalOrderId
                };
                var response = service.GetOrderByExternalId(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var order = MappingsFromService.ToGiftShopOrder(response.Order);
                return order;
            }
        }

        public GiftShopOrder CreateCustomOrder(string clientId, int partnerId, string externalOrderId, string articleId, string articleName, decimal priceBonus, decimal priceRur)
        {
            var parameters = new CreateCustomOrderParameters
            {
                ClientId = clientId,
                ExternalOrderId = externalOrderId,
                PartnerId = partnerId,
                Items = new[]
                {
                    new CustomOrderItem
                    {
                        ArticleId = articleId,
                        ArticleName = articleName,
                        Amount = 1,
                        PriceBonus = priceBonus,
                        PriceRur = priceRur
                    }
                }
            };

            using (var service = new OrderManagementServiceClient())
            {
                var response = service.CreateCustomOrder(parameters);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var order = MappingsFromService.ToGiftShopOrder(response.Order);

                return order;
            }
        }

        public void ChangeOrderStatus(string clientId, int orderId, OrderStatus status)
        {
            using (var service = new OrderManagementServiceClient())
            {
                var response = service.ChangeOrdersStatuses(new[]
                {
                    new OrdersStatus
                    {
                        ClientId = clientId,
                        OrderId = orderId,
                        OrderStatus = MappingsToService.ToOrderStatuses(status)
                    }
                });

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // Do nothing
        }

        #endregion

        private static void AssertResponse(int code, string message)
        {
            switch (code)
            {
                case 0:
                    return;
                case 2:
                    throw new OrderNotFoundException(code, message);
                case 600:
                    throw new BasketItemNotFoundException(code, message);
                case 700:
                    throw new OrderCannotBeDeliveredException(code, message);
                case 701:
                    throw new OrderItemNotAvailableException(code, message);
                case 410:
                case 420:
                    throw new OrderCancelledByPartnerException(code, message);
            }
            throw new OrdersServiceException(code, message);
        }
    }
}