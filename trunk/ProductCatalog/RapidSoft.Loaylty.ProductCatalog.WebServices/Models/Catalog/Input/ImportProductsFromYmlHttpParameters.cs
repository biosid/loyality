namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ImportProductsFromYmlHttpParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public string FullFilePath { get; set; }

        public WeightProcessTypes? WeightProcessType { get; set; }
    }
}
