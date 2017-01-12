namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class GetDeliveryVariantsParam
    {
        public int PartnerId { get; set; }

        public string ClientId { get; set; }

        public Location Location { get; set; }

        public OrderItem[] Items { get; set; }
    }
}