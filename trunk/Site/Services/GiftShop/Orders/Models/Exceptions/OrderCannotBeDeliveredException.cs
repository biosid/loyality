namespace Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions
{
    public class OrderCannotBeDeliveredException : OrdersServiceException
    {
        public OrderCannotBeDeliveredException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}