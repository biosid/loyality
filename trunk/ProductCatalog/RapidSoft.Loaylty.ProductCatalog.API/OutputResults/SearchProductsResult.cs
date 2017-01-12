namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class SearchProductsResult : ResultBase
    {
        [DataMember]
        public int MaxCountToTake { get; set; }

        [DataMember]
        public long? TotalCount { get; set; }

        [DataMember]
        public decimal? MaxPrice { get; set; }

        [DataMember]
        public Product[] Products { get; set; }

        public static SearchProductsResult BuildSuccess(
            IEnumerable<Product> products, long? totalCount = null, int maxCountToTake = 0, decimal? maxPrice = null)
        {
            var asArray = products.ToArray();
            return new SearchProductsResult
                       {
                           Products = asArray,
                           TotalCount = totalCount,
                           MaxPrice = maxPrice,
                           ResultCode = ResultCodes.SUCCESS,
                           MaxCountToTake = maxCountToTake
                       };
        }
    }
}
