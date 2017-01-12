using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToDeserializeXml : BonusGatewayException
    {
        public FailedToDeserializeXml(Exception innerException)
            : base("ошибка при десериализации из XML", innerException)
        {
        }
    }
}
