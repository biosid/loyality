namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public enum OrderStatus
    {
        Unknown = 0,
        Draft,
        Registration,
        // В обработке
        Processing,
        // Отклонено партнёром
        CancelledByPartner,
        // Ожидает доставки
        DeliveryWaiting,
        // Доставляется
        Delivery,
        // Доставлено
        Delivered,
        // Доставлено с опозданием
        DeliveredWithDelay,
        // Не доставлено
        NotDelivered,
    }
}
