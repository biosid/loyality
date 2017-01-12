using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public enum OrderPaymentStatuses
    {
        // ReSharper disable InconsistentNaming

        [Description("Не произведена")]
        pending,

        [Description("Произведена")]
        done,

        [Description("Не прошла")]
        error,

        [Description("Отменена банком")]
        bank_cancelled

        // ReSharper restore InconsistentNaming
    }

    public static class OrderPaymentStatusExtensions
    {
        public static OrderPaymentStatus Map(this OrderPaymentStatuses original)
        {
            switch (original)
            {
                case OrderPaymentStatuses.pending:
                    return OrderPaymentStatus.No;
                case OrderPaymentStatuses.done:
                    return OrderPaymentStatus.Yes;
                case OrderPaymentStatuses.error:
                    return OrderPaymentStatus.Error;
                case OrderPaymentStatuses.bank_cancelled:
                    return OrderPaymentStatus.BankCancelled;
            }
            return OrderPaymentStatus.No;
        }

        public static OrderPaymentStatuses? Map(this OrderPaymentStatus original)
        {
            switch (original)
            {
                case OrderPaymentStatus.No:
                    return OrderPaymentStatuses.pending;
                case OrderPaymentStatus.Yes:
                    return OrderPaymentStatuses.done;
                case OrderPaymentStatus.Error:
                    return OrderPaymentStatuses.error;
                case OrderPaymentStatus.BankCancelled:
                    return OrderPaymentStatuses.bank_cancelled;
            }
            return null;
        }

        // REVIEW: css классы никогда не засовываем в c#  код
        public static string GetLabelClass(this OrderPaymentStatuses status)
        {
            switch (status)
            {
                case OrderPaymentStatuses.done:
                    return "label-success";
                case OrderPaymentStatuses.error:
                case OrderPaymentStatuses.bank_cancelled:
                    return "label-important";
                default:
                    return "label-warning";
            }
        }
    }
}
