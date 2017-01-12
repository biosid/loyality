namespace OnlinePartnerStub.Models.Main
{
    public class ReturnModel
    {
        public string OrderId { get; set; }

        public int Status { get; set; }

        public decimal Discount { get; set; }

        public string Error { get; set; }

        public string UtcDateTime { get; set; }

        public string Signature { get; set; }
    }
}
