namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetDeliveryLocationHistoryParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Количество пропущенных записей.
        /// </summary>
        [DataMember]
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        [DataMember]
        public int? CountToTake { get; set; }

        /// <summary>
        /// Признак подсчета общего количества найденных записей.
        /// </summary>
        [DataMember]
        public bool? CalcTotalCount { get; set; }

        [IgnoreDataMember]
        public int? LocationId { get; set; }
    }
}