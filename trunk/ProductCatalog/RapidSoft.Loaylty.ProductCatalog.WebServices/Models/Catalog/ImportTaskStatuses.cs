namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Статус импорта
    /// </summary>
    [DataContract]
    public enum ImportTaskStatuses
    {
        /// <summary>
        /// Ожидание - задача на имопртирование каталога только создана и еще не обрабатывалась.
        /// </summary>
        [EnumMember]
        Waiting = 0,

        /// <summary>
        /// Загрузка xml-файла в каталог системы.
        /// </summary>
        [EnumMember]
        Loading = 2,

        /// <summary>
        /// Загрузка выполнена
        /// </summary>
        [EnumMember]
        Completed = 3,

        /// <summary>
        /// Загрузка отменена пользователем
        /// </summary>
        [EnumMember]
        Canceled = 4,

        /// <summary>
        /// Критическая ошибка не позволяющая загрузить хотя бы один товар, например, xml-файл не найден.
        /// </summary>
        [EnumMember]
        Error = 5,
    }
}
