namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class UpdateProductParameters : ProductParameters
    {
        [DataMember]
        public string ProductId { get; set; }
    }
}
