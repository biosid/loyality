using System;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Exceptions
{
    public class SecurityTokenServiceException :  Exception
    {
        public SecurityTokenServiceException(string message)
            : base("[Сервис токенов]: " + message)
        {
        }
    }
}