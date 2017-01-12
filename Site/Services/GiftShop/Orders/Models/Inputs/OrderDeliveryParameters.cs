namespace Vtb24.Site.Services.GiftShop.Orders.Models.Inputs
{
    public class OrderDeliveryParameters
    {
        public string ExternalDeliveryVariantId { get; set; }

        public DeliveryLocationInfo DeliveryVariantLocation { get; set; }

        public DeliveryContact Contact { get; set; }

        public string Comment { get; set; }

        public DeliveryType Type { get; set; }

        #region параметры для Type = DeliveryType.Delivery

        public DeliveryAddress Address { get; set; }

        #endregion

        #region параметры для Type = DeliveryType.Pickup

        public string ExternalPickupPointId { get; set; }

        #endregion
    }
}
