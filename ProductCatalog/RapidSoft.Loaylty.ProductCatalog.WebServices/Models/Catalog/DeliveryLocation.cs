namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Привязка - описывает соответствие населенного пункту (точки доставки) партнера и код КЛАДР.
    /// </summary>
    [DataContract]
    public class DeliveryLocation
    {
        /// <summary>
        /// Идентификатор привязки
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор партнера
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Населенный пункт: пользовательское описание точки доставки.
        /// </summary>
        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public string ExternalLocationId { get; set; }

        /// <summary>
        /// Код КЛАДР «привязанный» к «населенному пункту»
        /// </summary>
        [DataMember]
        public string Kladr { get; set; }

        /// <summary>
        /// Статус привязки
        /// </summary>
        [DataMember]
        public DeliveryLocationStatuses Status { get; set; }

        /// <summary>
        /// Дата и время добавления партнера
        /// </summary>
        [DataMember]
        public DateTime InsertDateTime { get; set; }

        /// <summary>
        /// Дата и время обновления привязки
        /// </summary>
        [DataMember]
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// Идентификатор пользователя обновившего привязки
        /// </summary>
        [DataMember]
        public string UpdateUserId { get; set; }

        [IgnoreDataMember]
        public DeliveryLocationUpdateSources UpdateSource { get; set; }
    }
}
