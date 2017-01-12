namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Partner
    {
        /// <summary>
        /// Внутренний идентификатор партнера
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Описание партнера
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Внутренний идентификатор клиета добавившего партнера
        /// </summary>
        [DataMember]
        public string InsertedUserId { get; set; }

        /// <summary>
        /// Внутренний идентификатор клиета обновившего партнера
        /// </summary>
        [DataMember]
        public string UpdatedUserId { get; set; }

        /// <summary>
        /// Наименование партнера
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Тип партнёра
        /// </summary>
        [DataMember]
        public PartnerType Type { get; set; }

        /// <summary>
        /// Статус партнёра
        /// </summary>
        [DataMember]
        public PartnerStatus Status { get; set; }

        /// <summary>
        /// Степень доверия партнёра
        /// </summary>
        [DataMember]
        public PartnerThrustLevel ThrustLevel { get; set; }

        /// <summary>
        /// Дата и время добавления партнера
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// Дата и время обновления партнера
        /// </summary>
        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Ссылка на ид курьера
        /// </summary>
        [DataMember]
        public int? CarrierId { get; set; }

        /// <summary>
        /// Является ли партнёр одновременно и курьером
        /// </summary>
        [DataMember]
        public bool IsCarrier { get; set; }

        /// <summary>
        /// Ссылка на объект курьера
        /// </summary>
        public Partner Carrier { get; set; }

        /// <summary>
        /// Настройки партнёра
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Settings { get; set; }
    }
}