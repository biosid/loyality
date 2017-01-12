namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
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
        public PaymentStatuses OrderPaymentStatus { get; set; }

        /// <summary>
        /// Статус доставки
        /// </summary>
        [DataMember]
        public PaymentStatuses DeliveryPaymentStatus { get; set; }
    }
}
