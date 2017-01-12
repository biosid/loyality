namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PopularProduct : PublicProduct
    {
        [DataMember]
        public int PopularityRate { get; set; }
    }
}
