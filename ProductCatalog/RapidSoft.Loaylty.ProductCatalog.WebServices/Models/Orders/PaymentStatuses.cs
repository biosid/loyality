namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PaymentStatuses
    {
        /// <summary>
        /// Отсутствует
        /// </summary>
        [EnumMember]
        No = 0,

        /// <summary>
        /// Оплата произведена
        /// </summary>
        [EnumMember]
        Yes = 1,

        /// <summary>
        /// Оплата не прошла
        /// </summary>
        [EnumMember]
        Error = 2,

        /// <summary>
        /// Оплата отменена банком
        /// </summary>
        [EnumMember]
        BankCancelled = 3
    }
}
