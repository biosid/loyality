namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class PickupPoint
    {
        public string Name { get; set; }

        public string ExternalPickupPointId { get; set; }

        public string DeliveryVariantName { get; set; }

        public string ExternalDeliveryVariantId { get; set; }

        public string Address { get; set; }

        public string[] Phones { get; set; }

        public string[] OperatingHours { get; set; }

        public string Description { get; set; }

        public decimal ItemsCost { get; set; }

        public decimal DeliveryCost { get; set; }

        public decimal TotalCost { get; set; }
    }
}