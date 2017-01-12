namespace Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions
{
    public class OrderCancelledByPartnerException : OrdersServiceException
    {
        public OrderCancelledByPartnerException(int resultCode, string codeDescription) : base(resultCode, codeDescription)
        {
        }
    }
}