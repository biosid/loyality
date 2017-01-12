namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class InvalidOrder : BonusGatewayException
    {
        public InvalidOrder(string error)
            : base("неверные данные заказа: " + error)
        {
        }
    }
}
