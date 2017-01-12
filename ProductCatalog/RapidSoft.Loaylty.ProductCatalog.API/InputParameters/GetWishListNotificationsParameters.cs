namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetWishListNotificationsParameters
    {
        /// <summary>
        /// Указывает надо ли пересобрать список уведомлений
        /// </summary>
        [DataMember]
        public bool? Rebuild { get; set; }

        /// <summary>
        /// Считать ли общее количество найденных категорий
        /// </summary>
        [DataMember]
        public bool? CalcTotalCount { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        [DataMember]
        public int? CountToTake { get; set; }

        public string ClientId { get; set; }
    }
}