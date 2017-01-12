using System;

namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class SecurityServiceException : Exception
    {
        public SecurityServiceException(string message) : base("[Подсистема безопасности]:" + message)
        {
        }
    }
}