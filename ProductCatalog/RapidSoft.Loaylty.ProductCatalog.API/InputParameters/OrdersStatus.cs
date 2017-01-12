namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class OrdersStatus
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int? OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// Статус заказа
        /// </summary>
        [DataMember]
        public OrderStatuses? OrderStatus
        {
            get;
            set;
        }

        [DataMember]
        public string ClientId
        {
            get;
            set;
        }

        [DataMember]
        public string OrderStatusDescription
        {
            get;
            set;
        }
    }
}