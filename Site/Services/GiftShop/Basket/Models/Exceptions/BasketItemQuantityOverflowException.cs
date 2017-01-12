using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions
{
    public class BasketItemQuantityOverflowException : BasketServiceException
    {
        public BasketItemQuantityOverflowException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
