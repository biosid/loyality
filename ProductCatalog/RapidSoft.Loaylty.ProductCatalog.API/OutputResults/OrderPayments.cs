namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class OrderPayments
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        [DataMember]
        public OrderPaymentStatuses PaymentStatus { get; set; }

        /// <summary>
        /// Статус доставки
        /// </summary>
        [DataMember]
        public OrderDeliveryPaymentStatus DeliveryPaymentStatus { get; set; }
    }
}