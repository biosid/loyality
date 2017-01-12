using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.Helpers;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class StatusHistoryRecordModel
    {
        public DateTime When { get; set; }

        public string Who { get; set; }

        public StatusHistoryRecordField[] Fields { get; set; }

        public static StatusHistoryRecordModel Map(OrderStatusHistoryRecord original)
        {
            return new StatusHistoryRecordModel
            {
                When = original.When,
                Who = original.Who,
                Fields = MapFields(original).ToArray()
            };
        }

        private static IEnumerable<StatusHistoryRecordField> MapFields(OrderStatusHistoryRecord original)
        {
            if (original.Status != null)
                yield return StatusHistoryRecordField.Map(original.Status, "Статус заказа", GetDescription);

            if (original.StatusDescription != null)
                yield return StatusHistoryRecordField.Map(original.StatusDescription, "Описание статуса заказа", desc => desc);

            if (original.ProductPaymentStatus != null)
                yield return StatusHistoryRecordField.Map(original.ProductPaymentStatus, "Оплата вознаграждения", GetDescription);

            if (original.DeliveryPaymentStatus != null)
                yield return StatusHistoryRecordField.Map(original.DeliveryPaymentStatus, "Оплата доставки", GetDescription);
        }

        private static string GetDescription(OrderStatus? status)
        {
            return MapAndGetDescription(status, OrderStatusesExtensions.Map);
        }

        private static string GetDescription(OrderPaymentStatus? status)
        {
            return MapAndGetDescription(status, OrderPaymentStatusExtensions.Map);
        }

        private static string MapAndGetDescription<T, TMapped>(T? value, Func<T, TMapped?> map)
            where T : struct
            where TMapped : struct
        {
            if (!value.HasValue)
                return "-";

            var mapped = map(value.Value);

            return mapped.HasValue ? mapped.Value.EnumDescription() : "-неподдерживаемое значение-";
        }
    }
}
