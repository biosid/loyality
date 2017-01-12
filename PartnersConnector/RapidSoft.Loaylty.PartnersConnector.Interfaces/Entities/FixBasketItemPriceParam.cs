namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class FixBasketItemPriceParam
    {
        public int PartnerId { get; set; }

        public string ClientId { get; set; }

        public string BasketItemId { get; set; }

        public string OfferId { get; set; }

        public string OfferName { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }
    }
}