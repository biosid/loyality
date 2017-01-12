namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class PaymentIsNotFound : BonusGatewayException
    {
        public PaymentIsNotFound()
            : base("списание по данному идентификатору не производилось")
        {
        }
    }
}
