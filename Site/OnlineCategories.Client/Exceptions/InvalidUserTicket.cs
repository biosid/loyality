namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class InvalidUserTicket : BonusGatewayException
    {
        public InvalidUserTicket()
            : base("недействительный временный код")
        {
        }
    }
}
