using System;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class StatusHistoryRecordField
    {
        public string What { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public static StatusHistoryRecordField Map<T>(OrderStatusHistoryField<T> original, string what, Func<T, string> toString)
        {
            return new StatusHistoryRecordField
            {
                What = what,
                OldValue = toString(original.OldValue),
                NewValue = toString(original.NewValue)
            };
        }

        public static StatusHistoryRecordField Map(OrderStatusHistoryField<string> original, string what)
        {
            return new StatusHistoryRecordField
            {
                What = what,
                OldValue = original.OldValue,
                NewValue = original.NewValue
            };
        }
    }
}
