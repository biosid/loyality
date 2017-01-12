namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetWishListResult : ResultBase
    {
        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        [DataMember]
        public int MaxCountToTake { get; set; }

        /// <summary>
        /// Общее количество найденных записей.
        /// </summary>
        [DataMember]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Отложенные товары.
        /// </summary>
        [DataMember]
        public WishListItem[] Items { get; set; }

        public static GetWishListResult BuildSuccess(int maxCountToTake, int? totalCount = 0, IEnumerable<WishListItem> items = null)
        {
            return new GetWishListResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           MaxCountToTake = maxCountToTake,
                           TotalCount = totalCount,
                           Items = items == null ? new WishListItem[]{} : items.ToArray()
                       };
        }

        public static GetWishListResult BuildError(string message)
        {
            return BuildError(ResultCodes.UNKNOWN_ERROR, message);
        }

        public static GetWishListResult BuildError(int code, string message)
        {
            return new GetWishListResult
                       {
                           ResultCode = code,
                           ResultDescription = null,
                           MaxCountToTake = 0,
                           TotalCount = 0,
                           Items = new WishListItem[] { }
                       };
        }
    }
}