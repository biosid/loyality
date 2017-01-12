namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetOrdersHistoryParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public DateTime? Start { get; set; }

        [DataMember]
        public DateTime? End { get; set; }

        [DataMember]
        public PublicOrderStatuses[] Statuses { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}