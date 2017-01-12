namespace Vtb24.Site.Models.Buy
{
    public class ConfirmOrderModel
    {
        public string UserTicket { get; set; }

        public int OrderDraftId { get; set; }

        public string OtpToken { get; set; }

        public bool IsIframe
        {
            get { return !string.IsNullOrEmpty(UserTicket); }
        }
    }
}
