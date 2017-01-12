using System;

namespace Vtb24.Site.Services.Buy.Models.Exceptions
{
    public class PaymentNotAuthorizedException : Exception
    {
        public PaymentNotAuthorizedException() : base("Платеж не авторизован")
        {
        }
    }
}
