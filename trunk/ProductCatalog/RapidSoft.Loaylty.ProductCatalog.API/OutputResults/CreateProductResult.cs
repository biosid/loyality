namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class CreateProductResult : ResultBase
    {
        [DataMember]
        public Product Product
        {
            get;
            set;
        }
    }
}
