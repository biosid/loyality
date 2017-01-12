using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions
{
    public class OrdersServiceException : ComponentException
    {
        public OrdersServiceException(int resultCode, string codeDescription)
            : base("Заказы", resultCode, codeDescription)
        {
        }
    }
}