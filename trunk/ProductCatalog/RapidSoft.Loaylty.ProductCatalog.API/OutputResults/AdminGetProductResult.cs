namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class AdminGetProductResult : ResultBase
    {
        [DataMember]
        public Product Product { get; set; }

        public static AdminGetProductResult BuildSuccess(Product product)
        {
            return new AdminGetProductResult
            {
                ResultCode = ResultCodes.SUCCESS,
                ResultDescription = null,
                Success = true,
                Product = product
            };
        }
    }
}
