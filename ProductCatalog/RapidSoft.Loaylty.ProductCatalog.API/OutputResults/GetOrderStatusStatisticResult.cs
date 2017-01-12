namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetOrderStatusStatisticResult : ResultBase
    {
        public IDictionary<OrderStatuses, int> Statistic
        {
            get;
            set;
        }

        public bool HasOrders
        {
            get
            {
                return Statistic != null && Statistic.Any(x => x.Value > 0);
            }

            set
            {
            }
        }

        public static GetOrderStatusStatisticResult BuildSuccess(IDictionary<OrderStatuses, int> statistic)
        {
            return new GetOrderStatusStatisticResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       Success = true,
                       ResultDescription = null,
                       Statistic = statistic
                   };
        }
    }
}