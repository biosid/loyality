namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs
{
    public class ChangeOrderStatusOptions
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public string StatusDescription { get; set; }
    }
}
