namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum CategoryPositionTypes
    {
        /// <summary>
        /// Вставить в категорию последней
        /// </summary>
        [EnumMember]
        Append = 0,

        /// <summary>
        /// Вставить в категорию первой
        /// </summary>
        [EnumMember]
        Prepend = 1,

        /// <summary>
        /// Вставить перед категорией
        /// </summary>
        [EnumMember]
        Before = 2,

        /// <summary>
        /// Вставить после категории
        /// </summary>
        [EnumMember]
        After = 3
    }
}
