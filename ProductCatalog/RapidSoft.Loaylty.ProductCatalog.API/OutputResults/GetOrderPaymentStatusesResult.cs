namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetOrderPaymentStatusesResult : ResultBase
    {
        public OrderPayments[] OrderPaymentStatuses { get; set; }

        public static GetOrderPaymentStatusesResult BuildSuccess(IEnumerable<Order> orders)
        {
            var dtos = orders == null ? new OrderPayments[0] : orders.Select(ToOrderPayments).ToArray();

            return new GetOrderPaymentStatusesResult
                       {
                           Success = true,
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           OrderPaymentStatuses = dtos
                       };
        }

        private static OrderPayments ToOrderPayments(Order order)
        {
            return new OrderPayments
                       {
                           Id = order.Id,
                           PaymentStatus = order.PaymentStatus,
                           DeliveryPaymentStatus = order.DeliveryPaymentStatus
                       };
        }
    }
}