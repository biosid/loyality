using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToValidateResponse : BonusGatewayException
    {
        public FailedToValidateResponse(Exception innerException)
            : base("ошибка при проверке подписи ответа", innerException)
        {

        }
    }
}
