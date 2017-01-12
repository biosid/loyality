using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LINQtoCSV;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    public class OrdersExport : IOrdersExport
    {
        private const int BATCH_LENGTH = 100;

        private readonly IOrders _orders;

        public OrdersExport(IOrders orders)
        {
            _orders = orders;
        }

        public void ExportOrdersHistoryPage(OrdersExportOptions options, TextWriter writer, int page, out int totalPages)
        {
            var orders = _orders.SearchOrders(options.SearchCriteria, PagingSettings.ByPage(page, BATCH_LENGTH));

            totalPages = orders.TotalPages;

            var csvContext = new CsvContext();

            var csvDescription = new CsvFileDescription
            {
                SeparatorChar = ';',
                FileCultureInfo = CultureInfo.GetCultureInfo("ru-ru"),
                EnforceCsvColumnAttribute = true,
                FirstLineHasColumnNames = page == 1
            };

            csvContext.Write(orders.SelectMany(order => ToOrderExportItems(order, options)), writer, csvDescription);
        }

        private static IEnumerable<OrderExportItem> ToOrderExportItems(Order original, OrdersExportOptions options)
        {
            var status = options.MapOrderStatus(original.Status);
            var productPaymentStatus = options.MapOrderPaymentStatus(original.ProductPaymentStatus);
            var deliveryPaymentStatus = options.MapOrderPaymentStatus(original.DeliveryPaymentStatus);

            var contactName = original.Delivery != null && original.Delivery.Contact != null
                                  ? ToContactName(original.Delivery.Contact)
                                  : string.Empty;
            var contactPhone = original.Delivery != null && original.Delivery.Contact != null
                                   ? original.Delivery.Contact.Phone
                                   : string.Empty;
            var contactEmail = original.Delivery != null && original.Delivery.Contact != null
                                   ? original.Delivery.Contact.Email
                                   : string.Empty;

            yield return new OrderExportItem
            {
                Id = original.Id,
                ExternalId = original.ExternalId,
                OrderTime = original.CreateDate,
                ProductId = "Оплата по заказу",
                ContactName = contactName,
                ContactPhone = contactPhone,
                ContactEmail = contactEmail,
                BonusPayment = original.TotalBonusPrice,
                RurPayment = original.TotalAdvance
            };

            yield return new OrderExportItem
            {
                Id = original.Id,
                ExternalId = original.ExternalId,
                OrderTime = original.CreateDate,
                Status = status,
                ProductPaymentStatus = productPaymentStatus,
                DeliveryPaymentStatus = deliveryPaymentStatus,
                ProductId = "Доставка",
                ContactName = contactName,
                ContactPhone = contactPhone,
                ContactEmail = contactEmail,
                Title = options.MapOrderDelivery(original.Delivery),
                Quantity = 1,
                Price = original.DeliveryPrice
            };

            foreach (var item in original.Items)
            {
                yield return new OrderExportItem
                {
                    Id = original.Id,
                    ExternalId = original.ExternalId,
                    OrderTime = original.CreateDate,
                    Status = status,
                    ProductPaymentStatus = productPaymentStatus,
                    DeliveryPaymentStatus = deliveryPaymentStatus,
                    ProductId = item.ProductId,
                    ContactName = contactName,
                    ContactPhone = contactPhone,
                    ContactEmail = contactEmail,
                    Title = item.Title,
                    Quantity = item.Quantity,
                    Price = original.IsBankProductOrder()
                                ? item.QuantityBonusPrice * OrderHelpers.BANK_PRODUCTS_PRICE_RATE
                                : item.QuantityPrice
                };
            }
        }

        private static string ToContactName(OrderDeliveryContact contact)
        {
            return string.Join(" ", ToContactNameParts(contact));
        }

        private static IEnumerable<string> ToContactNameParts(OrderDeliveryContact contact)
        {
            if (!string.IsNullOrEmpty(contact.LastName))
                yield return contact.LastName;
            if (!string.IsNullOrEmpty(contact.FirstName))
                yield return contact.FirstName;
            if (!string.IsNullOrEmpty(contact.MiddleName))
                yield return contact.MiddleName;
        }
    }
}
