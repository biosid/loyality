namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class InvalidResponseSignature : BonusGatewayException
    {
        public InvalidResponseSignature()
            : base("неверная подпись ответа")
        {
        }
    }
}
