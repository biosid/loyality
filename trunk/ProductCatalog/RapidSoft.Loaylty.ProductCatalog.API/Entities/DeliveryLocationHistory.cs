namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class DeliveryLocationHistory
    {
        public DeliveryLocationHistory()
        {
            this.NewStatus = DeliveryLocationStatus.CorrectBinded;
            this.OldStatus = DeliveryLocationStatus.CorrectBinded;
        }

        [IgnoreDataMember]
        public DateTime TriggerDate { get; set; }

        // NOTE: Не надо так как нужны только сделаные из АРМ, а это всегда UPDATE
        [IgnoreDataMember]
        public string Action { get; set; }

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

        // NOTE: Не надо так как нужны только сделаные из АРМ, а это всегда пользователь
        [IgnoreDataMember]
        public string EtlSessionId { get; set; }

        // NOTE: Не надо так как нужны только сделаные из АРМ, а это всегда UPDATE
        [IgnoreDataMember]
        public DateTime InsertDateTime { get; set; }

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
        public DeliveryLocationStatus NewStatus { get; set; }

        [DataMember]
        public DeliveryLocationStatus OldStatus { get; set; }
    }
}