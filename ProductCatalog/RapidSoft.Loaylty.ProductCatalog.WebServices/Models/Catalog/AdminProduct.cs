namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AdminProduct
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string PartnerProductId { get; set; }

        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string CategoryNamePath { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public ProductStatuses Status { get; set; }

        [DataMember]
        public ProductModerationStatuses ModerationStatus { get; set; }

        [DataMember]
        public bool IsRecommended { get; set; }

        [DataMember]
        public decimal PriceRur { get; set; }

        [DataMember]
        public string[] TargetAudienceIds { get; set; }

        [DataMember]
        public string[] Pictures { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public ProductParameterValue[] Parameters { get; set; }
    }
}
