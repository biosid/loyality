namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum OrderHistoryStatus
    {
        /// <summary>
        /// Изменился статус заказа
        /// </summary>
        Status = 0,

        /// <summary>
        /// Изменился статус оплаты
        /// </summary>
        Payment = 1,

        /// <summary>
        /// Изменился статус доставки
        /// </summary>
        Delivery = 2,
    }
}