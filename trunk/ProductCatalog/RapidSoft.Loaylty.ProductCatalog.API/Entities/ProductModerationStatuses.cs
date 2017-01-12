namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ProductModerationStatuses
    {
        /// <summary>
        /// на модерации
        /// </summary>
        [EnumMember]
        InModeration = 0,

        /// <summary>
        /// Модерация окончилась отказом
        /// </summary>
        [EnumMember]
        Canceled = 1,

        /// <summary>
        /// Модерация пройдена успешно
        /// </summary>
        [EnumMember]
        Applied = 2
    }
}
