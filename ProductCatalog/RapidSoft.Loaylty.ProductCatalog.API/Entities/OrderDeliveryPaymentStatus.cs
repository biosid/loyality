namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum OrderDeliveryPaymentStatus
    {
        /// <summary>
        /// Отсутствует
        /// </summary>
        No = 0,

        /// <summary>
        /// Оплата произведена
        /// </summary>
        Yes = 1,

        /// <summary>
        /// Оплата не прошла
        /// </summary>
        Error = 2,

        /// <summary>
        /// Оплата отменена банком
        /// </summary>
        BankCancelled = 3
    }
}