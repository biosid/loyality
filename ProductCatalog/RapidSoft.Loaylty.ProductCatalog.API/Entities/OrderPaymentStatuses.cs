namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum OrderPaymentStatuses
    {
        /// <summary>
        /// Отсутствует
        /// </summary>
        No = 0,

        /// <summary>
        /// Оплата за заказ произведена
        /// </summary>
        Yes = 1,

        /// <summary>
        /// Оплата за заказ не прошла
        /// </summary>
        Error = 2,

        /// <summary>
        /// Оплата отменена банком
        /// </summary>
        BankCancelled = 3
    }
}