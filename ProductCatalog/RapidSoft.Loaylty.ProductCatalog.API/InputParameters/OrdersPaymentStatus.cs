namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class OrdersPaymentStatus
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        [DataMember]
        public OrderPaymentStatuses PaymentStatus { get; set; }

        [DataMember]
        public string ClientId { get; set; }
    }
}