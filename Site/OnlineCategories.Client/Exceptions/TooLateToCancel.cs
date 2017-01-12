namespace Vtb24.OnlineCategories.Client.Exceptions
{
    public class TooLateToCancel : BonusGatewayException
    {
        public TooLateToCancel()
            : base("исчерпан лимит времени на отмену, отмена не может быть произведена")
        {
        }
    }
}
