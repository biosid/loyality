namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderHistory
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string UpdatedUserId { get; set; }

        [DataMember]
        public DateTime UpdatedDate { get; set; }

        [DataMember]
        public OrderStatuses? OldStatus { get; set; }

        [DataMember]
        public OrderStatuses? NewStatus { get; set; }

        [DataMember]
        public bool IsOrderStatusDescriptionChanged { get; set; }

        [DataMember]
        public string NewOrderStatusDescription { get; set; }

        [DataMember]
        public string OldOrderStatusDescription { get; set; }

        [DataMember]
        public PaymentStatuses? NewOrderPaymentStatus { get; set; }

        [DataMember]
        public PaymentStatuses? OldOrderPaymentStatus { get; set; }

        [DataMember]
        public PaymentStatuses? NewDeliveryPaymentStatus { get; set; }

        [DataMember]
        public PaymentStatuses? OldDeliveryPaymentStatus { get; set; }
    }
}
