namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderStatusHistoryField<T>
    {
        public T OldValue { get; set; }

        public T NewValue { get; set; }
    }
}
