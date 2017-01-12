namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class DeliveryLocationHistory
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

        /// <summary>
        /// Идентификатор пользователя обновившего привязки
        /// </summary>
        [DataMember]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// Дата и время обновления привязки
        /// </summary>
        [DataMember]
        public DateTime UpdateDateTime { get; set; }

        [DataMember]
        public string OldExternaLocationlId { get; set; }

        [DataMember]
        public string NewExternaLocationlId { get; set; }

        [DataMember]
        public string NewKladr { get; set; }

        [DataMember]
        public string OldKladr { get; set; }

        [DataMember]
        public DeliveryLocationStatuses NewStatus { get; set; }

        [DataMember]
        public DeliveryLocationStatuses OldStatus { get; set; }
    }
}
