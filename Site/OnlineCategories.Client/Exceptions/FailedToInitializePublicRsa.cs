using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToInitializePublicRsa : BonusGatewayException
    {
        public FailedToInitializePublicRsa(Exception innerException)
            : base("ошибка при инициализации RSA для BonusGateway", innerException)
        {

        }
    }
}
