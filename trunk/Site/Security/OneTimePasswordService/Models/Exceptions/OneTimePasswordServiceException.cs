using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class OneTimePasswordServiceException :  Exception
    {
        public OneTimePasswordServiceException(string message) : base("[Сервис OTP]: " + message)
        {
        }
    }
}