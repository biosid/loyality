namespace Vtb24.Site.Models.Buy
{
    public class DeliveryVariantsModel
    {
        public string LocationName { get; set; }

        public DeliveryGroupModel[] Groups { get; set; }

        public string DeliveryVariantId { get; set; }

        public string PickupPointId { get; set; }

        public bool ShowRurPrice { get; set; }
    }
}