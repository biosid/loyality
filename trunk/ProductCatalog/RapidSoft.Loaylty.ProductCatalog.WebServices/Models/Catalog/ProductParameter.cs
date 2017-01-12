namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductParameter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Unit { get; set; }
    }
}
