namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class GetSubCategoriesResult : ResultBase
    {
        [DataMember]
        public int MaxCountToTake { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public int ChildrenCount { get; set; }

        [DataMember]
        public ProductCategory[] Categories { get; set; }

        public static GetSubCategoriesResult BuildSuccess(
            IEnumerable<ProductCategory> categories, int maxCountToTake, int? totalCount, int childrenCount)
        {
            var asArray = categories.ToArray();
            return new GetSubCategoriesResult
                       {
                           Categories = asArray,
                           ChildrenCount = childrenCount,
                           MaxCountToTake = maxCountToTake,
                           ResultCode = ResultCodes.SUCCESS,
                           TotalCount = totalCount
                       };
        }
    }
}