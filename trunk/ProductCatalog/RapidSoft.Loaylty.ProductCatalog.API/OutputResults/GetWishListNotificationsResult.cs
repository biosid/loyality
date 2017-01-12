namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    public class GetWishListNotificationsResult : ResultBase
    {
        public Notification[] Notifications { get; set; }

        public int? TotalCount { get; set; }

        public static GetWishListNotificationsResult BuildSuccess(IEnumerable<Notification> notifications, int? totalCount = null)
        {
            var asArr = notifications.ToArray();
            return new GetWishListNotificationsResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       Success = true,
                       ResultDescription = null,
                       Notifications = asArr,
                       TotalCount = totalCount
                   };
        }

        public static new GetWishListNotificationsResult BuildFail(int resultCode, string resultDescription)
        {
            return new GetWishListNotificationsResult
            {
                ResultCode = resultCode,
                Success = false,
                ResultDescription = resultDescription
            };
        }

        public static GetWishListNotificationsResult BuildFail(ResultBase rebuildResult)
        {
            return new GetWishListNotificationsResult
                   {
                       ResultCode = rebuildResult.ResultCode,
                       Success = false,
                       ResultDescription = rebuildResult.ResultDescription
                   };
        }
    }
}