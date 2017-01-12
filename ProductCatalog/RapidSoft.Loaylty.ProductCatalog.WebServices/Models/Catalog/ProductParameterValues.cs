namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductParameterValues
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string[] Values { get; set; }
    }
}
