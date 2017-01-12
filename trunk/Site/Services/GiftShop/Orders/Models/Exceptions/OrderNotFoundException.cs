namespace Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions
{
    public class OrderNotFoundException : OrdersServiceException
    {
        public OrderNotFoundException(int resultCode, string codeDescription) : base(resultCode, codeDescription)
        {
        }
    }
}