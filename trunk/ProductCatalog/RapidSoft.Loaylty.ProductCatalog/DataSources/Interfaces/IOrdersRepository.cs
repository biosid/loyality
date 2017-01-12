namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using API.Entities;

    public interface IOrdersRepository
    {
        bool Exists(int? orderId, string externalOrderId = null);

        Order Get(int partnerId, string externalOrderId);

        IList<int> GetIds(OrderStatuses orderStatus, OrderPaymentStatuses orderPaymentStatus, PartnerType partnerType, int[] skipPartnerIds = null);

        Page<Order> GetForPayment(int countToSkip, int? countToTake = null);

        IList<OrderStatuses> GetNextOrderStatuses(OrderStatuses orderStatus);

        IList<OrderStatusWorkFlowItem> GetOrderStatusWorkFlow(OrderStatuses fromOrderStatus);

        IList<OrderStatusWorkFlowItem> GetOrderStatusWorkFlow(IEnumerable<OrderStatuses> fromOrderStatuses);

        bool HasNonterminatedOrders(string clientId);

        Order GetById(int orderId);

        void UpdateOrderStatusDescription(int orderId, string statusDescription, string updatedUserId);

        void UpdateDeliveryInstructions(int orderId, string deliveryInstructions, string updatedUserId);

        IList<Order> GetOrders(int[] ids);

        int[] GetOrdersWithAdvancePayments(int[] ids);
    }
}