using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToPerformHttpRequest : BonusGatewayException
    {
        public FailedToPerformHttpRequest(Exception innerException)
            : base("ошибка при выполнении HTTP-запроса", innerException)
        {

        }
    }
}
