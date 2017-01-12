namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class UnknownResponseStatus : BonusGatewayException
    {
        public UnknownResponseStatus(int status)
            : base(string.Format("неизвестный статус ответа: {0}", status))
        {
        }
    }
}
