namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using Entities;

    public class GetOrderHistoryResult : ResultBase
    {
        public OrderHistory[] OrderHistory { get; set; }

        public static GetOrderHistoryResult BuildSuccess(OrderHistory[] orderHistory)
        {
            return new GetOrderHistoryResult
            {
                ResultCode = ResultCodes.SUCCESS,
                ResultDescription = null,
                OrderHistory = orderHistory
            };
        }

        public static new GetOrderHistoryResult BuildFail(int code, string description)
        {
            return new GetOrderHistoryResult
            {
                ResultCode = code,
                ResultDescription = description,
            };
        }
    }
}