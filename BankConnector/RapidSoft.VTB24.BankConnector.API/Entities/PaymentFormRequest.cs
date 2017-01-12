namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    public class PaymentFormRequest
    {
        public string ClientId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string ReturnUrlSuccess { get; set; }

        public string ReturnUrlFail { get; set; }
    }
}
