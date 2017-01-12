using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class BonusGatewayException : Exception
    {
        public BonusGatewayException(string message, Exception innerException = null)
            : base("[BonusGateway] - " + message, innerException)
        {
        }
    }
}
