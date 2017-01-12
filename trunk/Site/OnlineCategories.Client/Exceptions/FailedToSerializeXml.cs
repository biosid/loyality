using System;

namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class FailedToSerializeXml : BonusGatewayException
    {
        public FailedToSerializeXml(Exception innerException)
            : base("ошибка при сериализации в XML", innerException)
        {
        }
    }
}
