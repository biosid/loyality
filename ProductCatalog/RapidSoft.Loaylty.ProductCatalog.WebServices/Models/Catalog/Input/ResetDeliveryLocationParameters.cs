namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResetDeliveryLocationParameters : CatalogAdminParameters
    {
        [DataMember]
        public int LocationId { get; set; }
    }
}
