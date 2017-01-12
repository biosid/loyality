namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;

    using API.Entities;

    using API.InputParameters;

    using API.OutputResults;

    /// <summary>
    ///     The OrdersDataSource interface.
    /// </summary>
    public interface IOrdersDataSource
    {
        #region Methods

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Insert(Order order);

        /// <summary>
        /// The update orders statuses.
        /// </summary>
        /// <param name="orderStatuseses">
        /// The order statuseses.
        /// </param>
        /// <param name="updatedUserId">
        /// The updated user id.
        /// </param>
        /// <returns>
        /// The <see cref="ChangeExternalOrderStatusResult"/>.
        /// </returns>
        ChangeOrderStatusResult[] UpdateOrdersStatuses(OrdersStatus[] orderStatuseses, string updatedUserId);

        /// <summary>
        /// The update orders payment statuses.
        /// </summary>
        /// <param name="statuses">
        /// The order payment statuseses.
        /// </param>
        /// <param name="updatedUserId">
        /// The updated user id.
        /// </param>
        /// <returns>
        /// The <see cref="ChangeExternalOrderStatusResult"/>.
        /// </returns>
        ChangeOrderStatusResult[] UpdateOrdersPaymentStatuses(OrdersPaymentStatus[] statuses, string updatedUserId);

        /// <summary>
        /// The update orders delivery statuses.
        /// </summary>
        /// <param name="statuses">
        /// The order delivery statuseses.
        /// </param>
        /// <param name="updatedUserId">
        /// The updated user id.
        /// </param>
        /// <returns>
        /// The <see cref="ChangeExternalOrderStatusResult"/>.
        /// </returns>
        ChangeOrderStatusResult[] UpdateOrdersDeliveryStatuses(OrdersDeliveryStatus[] statuses, string updatedUserId);

        /// <summary>
        /// The update external orders statuses.
        /// </summary>
        /// <param name="orderStatuseses">
        /// The external order statuseses.
        /// </param>
        /// <param name="updatedUserId">
        /// The updated user id.
        /// </param>
        /// <returns>
        /// The <see cref="ChangeExternalOrderStatusResult"/>.
        /// </returns>
        ChangeExternalOrderStatusResult[] UpdateExternalOrdersStatuses(ExternalOrdersStatus[] orderStatuseses, string updatedUserId);

        /// <summary>
        /// The get order.
        /// </summary>
        /// <param name="orderId">
        /// The order id.
        /// </param>
        /// <param name="clientId">
        /// UserId which order is belong
        /// </param>
        /// <returns>
        /// The <see cref="Order"/>.
        /// </returns>
        Order GetOrder(int orderId, string clientId = null);

        Order GetOrderByExternalId(string externalId, string clientId = null);

        /// <summary>
        /// The get orders.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="endDate">
        /// The end date.
        /// </param>
        /// <param name="statuses">
        /// The statuses.
        /// </param>
        /// <param name="skipStatuses">
        /// The skip statuses.
        /// </param>
        /// <param name="countToSkip">
        /// The count to skip.
        /// </param>
        /// <param name="countToTake">
        /// The count to take.
        /// </param>
        /// <param name="calcTotalCount">
        /// The calc total count.
        /// </param>
        /// <returns>
        /// The <see cref="OrderPage"/>.
        /// </returns>
        Page<Order> GetOrders(
            string clientId, 
            DateTime? startDate, 
            DateTime? endDate, 
            OrderStatuses[] statuses, 
            OrderStatuses[] skipStatuses, 
            int countToSkip, 
            int countToTake, 
            bool calcTotalCount);

        /// <summary>
        /// The get last delivery addresses.
        /// </summary>
        LastDeliveryAddress[] GetLastDeliveryAddresses(string clientId, bool excludeAddressesWithoutKladr, int? countToTake = null);

        /// <summary>
        /// The get orders.
        /// </summary>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="endDate">
        /// The end date.
        /// </param>
        /// <param name="statuses">
        /// The statuses.
        /// </param>
        /// <param name="skipStatuses">
        /// The skip statuses.
        /// </param>
        /// <param name="paymentStatuses">
        /// The payment status.
        /// </param>
        /// <param name="deliveryPaymentStatus">
        /// The delivery payment status.
        /// </param>
        /// <param name="partnerIds">
        /// Filter by Partner Ids.
        /// </param>
        /// <param name="countToSkip">
        /// The count to skip.
        /// </param>
        /// <param name="countToTake">
        /// The count to take.
        /// </param>
        /// <param name="calcTotalCount">
        /// The calc total count.
        /// </param>
        /// <param name="orderIds">
        /// order ids
        /// </param>
        /// <param name="carrierIds">
        /// The carrier Ids.
        /// </param>
        /// <returns>
        /// The <see cref="OrderPage"/>.
        /// </returns>
        Page<Order> SearchOrders(
            DateTime startDate,
            DateTime endDate,
            OrderStatuses[] statuses,
            OrderStatuses[] skipStatuses,
            OrderPaymentStatuses[] paymentStatuses,
            OrderDeliveryPaymentStatus[] deliveryPaymentStatus,
            int[] partnerIds,
            int countToSkip,
            int countToTake,
            bool calcTotalCount,
            int[] orderIds,
            int[] carrierIds);

        #endregion        
    }
}