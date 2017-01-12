using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToInitializePrivateRsa : BonusGatewayException
    {
        public FailedToInitializePrivateRsa(Exception innerException)
            : base("ошибка при инициализации RSA для партнера", innerException)
        {
            
        }
    }
}
