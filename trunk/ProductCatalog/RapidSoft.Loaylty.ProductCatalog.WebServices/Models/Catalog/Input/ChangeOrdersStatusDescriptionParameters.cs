namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeOrdersStatusDescriptionParameters : CatalogAdminParameters
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
