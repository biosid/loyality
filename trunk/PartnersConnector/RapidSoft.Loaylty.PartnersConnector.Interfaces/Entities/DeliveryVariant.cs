namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class DeliveryVariant
    {
        public string DeliveryVariantName { get; set; }

        public string ExternalDeliveryVariantId { get; set; }

        public PickupPoint[] PickupPoints { get; set; }

        public string Description { get; set; }

        public decimal ItemsCost { get; set; }

        public decimal DeliveryCost { get; set; }

        public decimal TotalCost { get; set; }
    }
}