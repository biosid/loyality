namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    public class CardRegistrationParameters
    {
        public string ShopId { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string Sum { get; set; }
        public string Signature { get; set; }
    }
}
