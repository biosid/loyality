namespace Vtb24.Site.Services.Buy.Models.Inputs
{
    public class ConfirmOrderParams
    {
        public int OrderId { get; set; }

        public string OtpToken { get; set; }
    }
}