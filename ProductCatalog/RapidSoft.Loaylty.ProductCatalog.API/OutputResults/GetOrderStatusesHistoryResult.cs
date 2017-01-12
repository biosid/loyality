namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using Entities;

    public class GetOrderStatusesHistoryResult : ResultBase
    {
        public OrderHistory[] OrderHistory { get; set; }

        public int? TotalCount { get; set; }

        public GetOrderStatusesHistoryResult()
        {
            OrderHistory = new OrderHistory[] { };
        }

        public static GetOrderStatusesHistoryResult BuildSuccess(OrderHistory[] orderHistory, int? totalCount, string description = null)
        {
            return new GetOrderStatusesHistoryResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true,
                           OrderHistory = orderHistory,
                           TotalCount = totalCount
                       };
        }

        public static new GetOrderStatusesHistoryResult BuildFail(int code, string description)
        {
            return new GetOrderStatusesHistoryResult
            {
                ResultCode = code,
                ResultDescription = description,
            };
        }
    }
}