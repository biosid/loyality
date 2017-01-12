namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderHistory
    {
        private bool? isOrderStatusDescriptionChanged;

        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор партнёра.
        /// </summary>
        [DataMember]
        public string UpdatedUserId { get; set; }

        /// <summary>
        /// Дата изменения статуса
        /// </summary>
        [DataMember]
        public DateTime UpdatedDate { get; set; }

        [DataMember]
        public OrderStatuses? OldStatus { get; set; }

        [DataMember]
        public OrderStatuses? NewStatus { get; set; }

        [DataMember]
        public bool IsOrderStatusDescriptionChanged
        {
            get
            {
                return this.isOrderStatusDescriptionChanged ?? NewOrderStatusDescription != OldOrderStatusDescription;
            }

            set
            {
                this.isOrderStatusDescriptionChanged = value;
            }
        }

        [DataMember]
        public string NewOrderStatusDescription { get; set; }

        [DataMember]
        public string OldOrderStatusDescription { get; set; }

        [DataMember]
        public OrderPaymentStatuses? NewOrderPaymentStatus { get; set; }

        [DataMember]
        public OrderPaymentStatuses? OldOrderPaymentStatus { get; set; }

        [DataMember]
        public OrderDeliveryPaymentStatus? NewDeliveryPaymentStatus { get; set; }

        [DataMember]
        public OrderDeliveryPaymentStatus? OldDeliveryPaymentStatus { get; set; }
    }
}