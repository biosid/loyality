namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ChangeStatusByPartnerParameters
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public string[] PartnerProductIds { get; set; }

        [DataMember]
        public ProductStatuses ProductStatus { get; set; }
    }
}
