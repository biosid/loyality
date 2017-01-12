using System;

namespace Vtb24.Site.Services.Buy.Models.Exceptions
{
    public class OrderPriceChangedException : Exception
    {
        public decimal TotalPrice { get; private set; }

        public OrderPriceChangedException(decimal totalPrice) 
        : base(CreateMessage(totalPrice))
        {
            TotalPrice = totalPrice;
        }

        private static string CreateMessage(decimal totalPrice)
        {
            return string.Format(
                "Цена вознаграждений заказа изменилась. Текущая цена: {0:N2}", 
                totalPrice
            );
        }
    }
}
