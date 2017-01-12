namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Extension;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;

    using CatalogAdmin = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using PhoneNumber = RapidSoft.VTB24.BankConnector.Entity.PhoneNumber;

    /// <summary>
    /// Обработчик заказов
    /// </summary>
    public class SendOrdersProcessor : ProcessorBase
    {
        public const string UnitellerShopIdPartnerKey = "UnitellerShopId";

        private readonly IOrderManagementService orderManagementService;
        private readonly IPaymentService paymentService;
        private readonly CatalogAdmin.ICatalogAdminService catalogAdminService;

        public SendOrdersProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IOrderManagementService orderManagementService,
            CatalogAdmin.ICatalogAdminService catalogAdminService,
            IPaymentService paymentService)
            : base(logger, uow)
        {
            this.orderManagementService = orderManagementService;
            this.catalogAdminService = catalogAdminService;
            this.paymentService = paymentService;
        }

        public static OrderForPayment Map(Order order)
        {
            var result = new OrderForPayment();

            var contact = (order.DeliveryInfo != null && order.DeliveryInfo.Contact != null) ? order.DeliveryInfo.Contact : null;
            
            result.OrderId = order.Id;
            result.PartnerId = order.PartnerId;
            result.PartnerOrderNum = order.ExternalOrderId;
            result.ClientId = order.ClientId;
            result.ArticleId = "В таблице OrderItemsForPayment";
            result.ArticleName = "В таблице OrderItemsForPayment";
            result.Amount = 1;
            result.OrderBonusCost = order.BonusTotalCost;
            result.OrderTotalCost = order.TotalCost;
            result.OrderDateTime = order.InsertedDate;

            if (order.DeliveryInfo != null)
            {
                result.DeliveryRegion = order.DeliveryInfo.Address.Maybe(i => i.RegionTitle);
                result.DeliveryCity = order.DeliveryInfo.Address.Maybe(i => i.CityTitle);
                result.DeliveryAddress = string.Format("{0}. {1}", order.DeliveryInfo.DeliveryVariantName, GetAddress(order));
            }

            result.ContactName = (contact != null) ? string.Format("{0} {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName) : null;
            result.ContactPhone = (contact != null && contact.Phone != null) ? PhoneNumber.FromServicePhone(contact.Phone).GlobalNumber : null;
            result.ContactEmail = (contact != null) ? contact.Email : null;
            result.CurrierId = order.CarrierId ?? order.PartnerId;
            result.OrderDeliveryCost = order.DeliveryAdvance > 0 ? 0 : order.DeliveryCost;
            result.OrderItemsCost = order.ItemsCost;

            return result;
        }

        public static OrderItemsForPayment[] MapOrderItems(Order order, PaymentInfo advancePayment)
        {
            var contact = (order.DeliveryInfo != null && order.DeliveryInfo.Contact != null) ? order.DeliveryInfo.Contact : null;
            var deliveryRegion = (order.DeliveryInfo != null) ? order.DeliveryInfo.Address.Maybe(i => i.RegionTitle) : null;
            var deliveryCity = (order.DeliveryInfo != null) ? order.DeliveryInfo.Address.Maybe(i => i.CityTitle) : null;
            var deliveryAddress = (order.DeliveryInfo != null) ? string.Format("{0}. {1}", order.DeliveryInfo.DeliveryVariantName, GetAddress(order)) : null;
            var contactName = (contact != null) ? string.Format("{0} {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName) : null;
            var contactPhone = (contact != null && contact.Phone != null) ? PhoneNumber.FromServicePhone(contact.Phone).GlobalNumber : null;
            var contactEmail = (contact != null) ? contact.Email : null;

            var items = GenerateOrderItems(order, advancePayment).ToArray();
            var index = 0;
            var orderItemIdPreffix = order.Id.ToString("D") + "_";

            foreach (var item in items)
            {
                item.OrderId = order.Id;
                item.OrderItemId = orderItemIdPreffix + index.ToString("D");
                item.PartnerId = order.PartnerId;
                item.PartnerOrderNum = order.ExternalOrderId;
                item.ClientId = order.ClientId;
                item.DeliveryRegion = deliveryRegion;
                item.DeliveryCity = deliveryCity;
                item.DeliveryAddress = deliveryAddress;
                item.ContactName = contactName;
                item.ContactPhone = contactPhone;
                item.ContactEmail = contactEmail;
                item.OrderDateTime = order.InsertedDate;

                ++index;
            }

            return items;
        }

        public static IEnumerable<OrderItemsForPayment> GenerateOrderItems(Order order, PaymentInfo advancePayment)
        {
            if (order.DeliveryInfo != null)
            {
                yield return new OrderItemsForPayment
                {
                    ArticleId = string.Empty,
                    ArticleName = "Доставка",
                    Amount = 1,
                    OrderBonusCost = order.BonusDeliveryCost,
                    OrderTotalCost = advancePayment != null ? 0 : order.DeliveryCost,
                    Advance = advancePayment != null ? order.DeliveryAdvance : (decimal?)null,
                    AdvancePOSId = advancePayment != null ? advancePayment.UnitellerShopId : string.Empty,
                    AdvanceRRN = advancePayment != null ? advancePayment.UnitellerBillNumber : string.Empty
                };
            }

            foreach (var item in order.Items)
            {
                yield return new OrderItemsForPayment
                {
                    ArticleId = item.Product.ProductId,
                    ArticleName = item.Product.Name,
                    Amount = item.Amount,
                    OrderBonusCost = advancePayment != null ? 0 : item.AmountPriceBonus,
                    OrderTotalCost = advancePayment != null ? 0 : item.AmountPriceRur,
                    AdvancePOSId = advancePayment != null ? advancePayment.UnitellerShopId : string.Empty,
                    AdvanceRRN = advancePayment != null ? advancePayment.UnitellerBillNumber : string.Empty
                };
            }

            if (advancePayment != null)
            {
                // В случае частичной оплаты order.BonusTotalCost будет содержать сумму бонусов,
                // которые клиент потратил на позиции, так как доставка всегда оплачивается полностью
                yield return new OrderItemsForPayment
                {
                    ArticleId = string.Empty,
                    ArticleName = "Итого",
                    Amount = 1,
                    Advance = order.ItemsAdvance,
                    OrderBonusCost = order.BonusTotalCost,
                    OrderTotalCost = order.TotalCost - order.TotalAdvance,
                    AdvancePOSId = advancePayment.UnitellerShopId,
                    AdvanceRRN = advancePayment.UnitellerBillNumber
                };
            }
        }

        public void Execute()
        {
            try
            {
                var changeOrdersResult = this.orderManagementService.ChangeOrdersStatusesBeforePayment();

                if (!changeOrdersResult.Success)
                {
                    var mess = string.Format("Ошибка при обновлении статусов заказов перед оплатой: {0} ", changeOrdersResult.ResultDescription);
                    Logger.Error(mess);
                    throw new Exception(mess);
                }

                var takeCount = ConfigHelper.BatchSize;

                Order[] newOrders;

                var totalCount = 0;
                var waitingCount = 0;
                var sendCount = 0;
                var errorCount = 0;

                while ((newOrders = this.GetOrders(totalCount, takeCount)).Any())
                {
                    var partners = GetPartnersByOrders(newOrders).ToArray();

                    foreach (var externalOrder in newOrders)
                    {
                        var orderId = externalOrder.Id;

                        try
                        {
                            // получаем партнера и проверяем заказ
                            var partner = GetPartner(partners, externalOrder.PartnerId, orderId);
                            externalOrder.Validate(partner);

                            // получаем информацию о платеже картой
                            PaymentInfo advancePayment = null;
                            if (externalOrder.DeliveryAdvance > 0)
                            {
                                var paymentInfoResponse = paymentService.GetPaymentByOrderId(orderId);
                                if (paymentInfoResponse == null ||
                                    !paymentInfoResponse.Success ||
                                    paymentInfoResponse.Result == null)
                                {
                                    throw new InvalidOperationException("ошибка получения информации о платеже картой для заказа " + orderId.ToString("D"));
                                }

                                advancePayment = paymentInfoResponse.Result;

                                if (string.IsNullOrWhiteSpace(advancePayment.UnitellerShopId) ||
                                    string.IsNullOrWhiteSpace(advancePayment.UnitellerBillNumber))
                                {
                                    throw new InvalidOperationException("информация о платеже картой не содержит данные uniteller для заказа " + orderId.ToString("D"));
                                }
                            }

                            // ищем последний запрос в банк по этому заказу
                            var lastSentItem = Uow.OrderForPaymentRepository
                                                  .GetAll()
                                                  .Where(item => item.OrderId == orderId)
                                                  .OrderByDescending(item => item.InsertedDate)
                                                  .FirstOrDefault();

                            if (lastSentItem != null)
                            {
                                var lastSentTime = lastSentItem.InsertedDate;

                                // ищем ответ банка на этот запрос
                                var response = Uow.OrderPaymentResponseRepository
                                                  .GetAll()
                                                  .FirstOrDefault(item => item.OrderId == orderId &&
                                                                          item.InsertedDate > lastSentTime);

                                if (response == null)
                                {
                                    // ответа нет, значит еще один запрос отправлять пока не нужно
                                    ++waitingCount;
                                    continue;
                                }

                                if (response.Status != (int)ReceivedOrderStatus.Cancelled)
                                {
                                    // залогируем предупреждение:
                                    // если заказ не был отменен банком, то он не должен попасть в список на выгрузку
                                    Logger.WarnFormat("Заказ {0} попал в список на выгрузку, хотя банк ответил по нему статусом {1}", orderId, response.Status);
                                }
                            }

                            // Создаём заказ
                            var order = Map(externalOrder);
                            var orderItems = MapOrderItems(externalOrder, advancePayment);

                            // Получаем параметры юнителлера для заказа
                            var unitellerItemsShopId = GetUnitellerShopId(partner, orderId);
                            var unitellerDeliveryShopId = unitellerItemsShopId;

                            if (externalOrder.CarrierId.HasValue &&
                                externalOrder.CarrierId.Value != externalOrder.PartnerId)
                            {
                                var carrier = GetPartner(partners, externalOrder.CarrierId.Value, orderId);
                                unitellerDeliveryShopId = GetUnitellerShopId(carrier, orderId);
                            }

                            var etlSessionId = Guid.Parse(this.Logger.EtlSessionId);
                            var posId = unitellerItemsShopId;

                            order.UnitellerItemsShopId = unitellerItemsShopId;
                            order.UnitellerDeliveryShopId = unitellerDeliveryShopId;
                            order.EtlSessionId = etlSessionId;
                            order.POSId = posId;

                            Uow.OrderForPaymentRepository.Add(order);

                            foreach (var orderItem in orderItems)
                            {
                                orderItem.UnitellerItemsShopId = unitellerItemsShopId;
                                orderItem.UnitellerDeliveryShopId = unitellerDeliveryShopId;
                                orderItem.EtlSessionId = etlSessionId;
                                orderItem.POSId = posId;

                                Uow.OrderItemsForPaymentRepository.Add(orderItem);
                            }

                            ++sendCount;
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorFormat(ex, "Ошибка при обработке заказа {0} ", orderId);
                            ++errorCount;
                        }
                    }

                    totalCount += newOrders.Length;
                }

                Uow.Save();

                Logger.Counter("Заказы", "всего", totalCount);
                Logger.Counter("Заказы", "с ошибками", errorCount);
                Logger.Counter("Заказы", "отправлены ранее", waitingCount);
                Logger.Counter("Заказы", "на отправку", sendCount);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        private static string GetAddress(Order order)
        {
            switch (order.DeliveryInfo.DeliveryType)
            {
                case DeliveryTypes.Delivery:
                    return order.DeliveryInfo.Address.Maybe(i => i.AddressText);
                case DeliveryTypes.Pickup:
                    return order.DeliveryInfo.PickupPoint.Maybe(i => i.Address).StripTags();
                case DeliveryTypes.Email:
                    return order.DeliveryInfo.Contact.Email;
            }

            throw new NotSupportedException("Тип доставки не поддерживается: " + order.DeliveryInfo.DeliveryType);
        }

        private string GetUnitellerShopId(CatalogAdmin.Partner partner, int orderId)
        {
            if (partner.Settings == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                    "Партнёр:{0} заказ:{1} Settings is null",
                    partner.Id,
                    orderId));
            }

            string unitellerShopId;
            partner.Settings.TryGetValue(UnitellerShopIdPartnerKey, out unitellerShopId);

            if (string.IsNullOrEmpty(unitellerShopId))
            {
                throw new InvalidOperationException(
                    string.Format(
                    "Партнёр:{0} заказ:{1} не задан {2}", 
                    partner.Id, 
                    orderId, 
                    UnitellerShopIdPartnerKey));
            }
            else if (unitellerShopId.Length > FieldLenght.UnitellerItemsShopIdMaxLen)
            {
                throw new InvalidOperationException(
                    string.Format(
                    "Партнёр:{0} заказ:{1} длинна {2} более {3}", 
                    partner.Id, 
                    orderId, 
                    UnitellerShopIdPartnerKey, 
                    FieldLenght.UnitellerItemsShopIdMaxLen));
            }

            return unitellerShopId;
        }

        private CatalogAdmin.Partner GetPartner(IEnumerable<CatalogAdmin.Partner> partners, int partnerId, int orderId)
        {
            var partner = partners.FirstOrDefault(p => p.Id == partnerId);

            if (partner == null)
            {
                throw new InvalidOperationException(
                    string.Format("Парнёр (курьер) {0} не найден для заказа {1}", partnerId, orderId));
            }

            return partner;
        }
        
        private Order[] GetOrders(int countToSkip, int takeCount)
        {
            var getOrdersForPaymentParameters = new GetOrdersForPaymentParameters
            {
                CountToSkip = countToSkip,
                CountToTake = takeCount
            };
            var result = orderManagementService.GetOrdersForPayment(getOrdersForPaymentParameters);

            if (result == null)
            {
                Logger.ErrorFormat("OrderManagementService не вернул ответа, был отправлен запрос skip ({0}), take ({1})", countToSkip, takeCount);
                return new Order[0];
            }

            if (result.ResultCode != 0 || !result.Success)
            {
                Logger.ErrorFormat("Запрос OrderManagementService вернул ответ с ошибкой ({0} - {1})", result.ResultCode, result.ResultDescription);
            }

            if (result.Orders == null)
            {
                Logger.ErrorFormat("В ответе OrderManagementService отсутствует список с заказами, был отправлен запрос skip ({0}), take ({1})", countToSkip, takeCount);
            }

            return result.Orders;
        }

        private IEnumerable<CatalogAdmin.Partner> GetPartnersByOrders(Order[] orders)
        {
            var partnerIds = orders.Select(x => x.PartnerId)
                      .Union(orders.Where(x => x.CarrierId.HasValue).Select(x => x.CarrierId.Value))
                      .Distinct()
                      .ToArray();

            var partnersResult = catalogAdminService.GetPartners(partnerIds, ConfigHelper.VtbSystemUser);

            if (!partnersResult.Success)
            {
                var mess = string.Format("Ошибка при получении списка партнеров для оплаты в uniteller ({0} - {1})", partnersResult.ResultCode, partnersResult.ResultDescription);
                Logger.Error(mess);
                throw new Exception(mess);
            }

            return partnersResult.Partners.ToList();
        }
    }
}
