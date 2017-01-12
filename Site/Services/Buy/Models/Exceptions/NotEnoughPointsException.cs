using System;

namespace Vtb24.Site.Services.Buy.Models.Exceptions
{
    public class NotEnoughPointsException : Exception
    {
        public decimal Balance { get; private set; }

        public decimal TotalPrice { get; private set; }

        public NotEnoughPointsException(decimal balance, decimal total) : base(CreateMessage(balance, total))
        {
            Balance = balance;
            TotalPrice = total;
        }

        private static string CreateMessage(decimal balance, decimal total)
        {
            return string.Format(
                "Недостаточно бонусов для оплаты заказа. Баланс: {0}, цена: {1}", 
                balance,
                total
            );
        }
    }
}