namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class DeliveryGroup
    {
        public string GroupName { get; set; }

        public DeliveryVariant[] DeliveryVariants { get; set; }
    }
}