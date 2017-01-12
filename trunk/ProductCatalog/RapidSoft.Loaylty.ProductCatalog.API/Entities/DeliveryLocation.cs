namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Привязка - описывает соответствие населенного пункту (точки доставки) партнера и код КЛАДР.
    /// </summary>
    [DataContract]
    public class DeliveryLocation
    {
        public DeliveryLocation()
        {
            this.Status = DeliveryLocationStatus.NotCorrectKladr;
        }

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
        public DeliveryLocationStatus Status { get; set; }

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
        public UpdateSources UpdateSource { get; set; }
    }

    public enum UpdateSources
    {
        Unknow = 0,
        Arm = 1,
        Import = 2
    }
}