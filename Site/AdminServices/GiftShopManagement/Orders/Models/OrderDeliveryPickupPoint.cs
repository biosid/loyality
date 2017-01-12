namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderDeliveryPickupPoint
    {
        public string Name { get; set; }

        public string ExternalPickupPointId { get; set; }

        public string ExternalDeliveryVariantId { get; set; }

        public string Address { get; set; }

        public string[] Phones { get; set; }

        public string[] OperatingHours { get; set; }

        public string Description { get; set; }
    }
}
