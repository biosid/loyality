namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetCategoryProductParamsParameters
    {
        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public ProductParameter[] ProductParameters { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }
    }
}
