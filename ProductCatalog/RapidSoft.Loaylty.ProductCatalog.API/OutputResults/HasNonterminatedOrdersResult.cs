namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class HasNonterminatedOrdersResult : ResultBase
    {
        public bool HasOrders { get; set; }

        public static HasNonterminatedOrdersResult BuildSuccess(bool hasOrders)
        {
            return new HasNonterminatedOrdersResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       Success = true,
                       ResultDescription = null,
                       HasOrders = hasOrders
                   };
        }
    }
}