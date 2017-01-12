using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToSignRequest : BonusGatewayException
    {
        public FailedToSignRequest(Exception innerException)
            : base("ошибка при создании подписи запроса", innerException)
        {

        }
    }
}
