namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class OperationError : BonusGatewayException
    {
        public OperationError(string error)
            : base("при выполнение операции произошла ошибка: " + error)
        {
        }
    }
}
