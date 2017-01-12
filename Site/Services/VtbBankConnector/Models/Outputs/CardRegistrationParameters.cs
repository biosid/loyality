namespace Vtb24.Site.Services.VtbBankConnector.Models.Outputs
{
    public class CardRegistrationParameters
    {
        public string ShopId { get; set; }

        public string OrderId { get; set; }

        public string Subtotal { get; set; }

        public string CustomerId { get; set; }

        public string Signature { get; set; }
    }
}
