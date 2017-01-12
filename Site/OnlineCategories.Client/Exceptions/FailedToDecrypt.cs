using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToDecrypt : BonusGatewayException
    {
        public FailedToDecrypt(Exception innerException)
            : base("ошибка при расшифровке", innerException)
        {

        }
    }
}
