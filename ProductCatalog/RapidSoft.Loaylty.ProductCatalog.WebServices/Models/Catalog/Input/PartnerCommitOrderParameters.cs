namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;

    [DataContract]
    public class PartnerCommitOrderParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public PartnerOrderCommitment[] Commitments { get; set; }
    }
}
