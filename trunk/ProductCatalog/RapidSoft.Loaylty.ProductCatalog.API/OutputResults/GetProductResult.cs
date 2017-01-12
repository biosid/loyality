namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class GetProductResult : ResultBase
    {
        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public ProductAvailabilityStatuses AvailabilityStatus { get; set; }

        [DataMember]
        public int ViewsCount { get; set; }

        public static GetProductResult BuildSuccess(Product product)
        {
            return new GetProductResult
            {
                ResultCode = ResultCodes.SUCCESS,
                ResultDescription = null,
                Success = true,
                Product = product
            };
        }
    }
}
