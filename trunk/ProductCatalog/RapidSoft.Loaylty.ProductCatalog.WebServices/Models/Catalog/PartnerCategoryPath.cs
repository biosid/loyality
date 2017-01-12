namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PartnerCategoryPath
    {
        [DataMember]
        public bool IncludeSubCategories { get; set; }

        [DataMember]
        public string NamePath { get; set; }
    }
}
