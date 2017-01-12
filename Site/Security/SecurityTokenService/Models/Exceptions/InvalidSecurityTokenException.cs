using System;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Exceptions
{
    public class InvalidSecurityTokenException : SecurityTokenServiceException
    {
        public string SecurityToken { get; set; }

        public InvalidSecurityTokenException(string securityToken)
            : base(CreateMessage(securityToken))
        {
            SecurityToken = securityToken;
        }

        private static string CreateMessage(string securityToken)
        {
            return string.Format("Неправильный токен ({0}).", securityToken);
        }
    }
}