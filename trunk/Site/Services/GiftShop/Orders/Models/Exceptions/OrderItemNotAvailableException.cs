namespace Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions
{
    public class OrderItemNotAvailableException : OrdersServiceException
    {
        public OrderItemNotAvailableException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}