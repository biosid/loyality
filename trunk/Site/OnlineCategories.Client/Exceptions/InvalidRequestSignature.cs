namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class InvalidRequestSignature : BonusGatewayException
    {
        public InvalidRequestSignature()
            : base("неверная подпись запроса")
        {
        }
    }
}
