namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class OrdersDeliveryStatus
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// Статус доставки
        /// </summary>
        [DataMember]
        public OrderDeliveryPaymentStatus DeliveryStatus { get; set; }

        [DataMember]
        public string ClientId { get; set; }
    }
}