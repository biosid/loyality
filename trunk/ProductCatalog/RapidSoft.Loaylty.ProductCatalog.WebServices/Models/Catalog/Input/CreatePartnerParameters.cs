namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class CreatePartnerParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Название партнера.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Описание партнера.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Тип партнёра
        /// </summary>
        [DataMember]
        public PartnerTypes Type { get; set; }

        /// <summary>
        /// Статус партнёра
        /// </summary>
        [DataMember]
        public PartnerStatuses Status { get; set; }

        /// <summary>
        /// Степень доверия партнёра
        /// </summary>
        [DataMember]
        public PartnerTrustLevels ThrustLevel { get; set; }

        /// <summary>
        /// Идентификатор курьера
        /// </summary>
        [DataMember]
        public int? CarrierId { get; set; }

        /// <summary>
        /// Признак что партнёр курьер
        /// </summary>
        [DataMember]
        public bool IsCarrier { get; set; }

        [DataMember]
        public Dictionary<string, string> Settings { get; set; }
    }
}