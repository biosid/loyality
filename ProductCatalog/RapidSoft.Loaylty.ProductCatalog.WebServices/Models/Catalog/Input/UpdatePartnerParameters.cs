namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class UpdatePartnerParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Внутренний идентификатор партнера
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Название партнера.
        /// </summary>
        [DataMember]
        public string NewName { get; set; }

        /// <summary>
        /// Описание партнера.
        /// </summary>
        [DataMember]
        public string NewDescription { get; set; }

        /// <summary>
        /// Тип партнёра
        /// </summary>
        [DataMember]
        public PartnerTypes NewType { get; set; }

        /// <summary>
        /// Статус партнёра
        /// </summary>
        [DataMember]
        public PartnerStatuses NewStatus { get; set; }

        /// <summary>
        /// Степень доверия партнёра
        /// </summary>
        [DataMember]
        public PartnerTrustLevels NewThrustLevel { get; set; }

        /// <summary>
        /// Идентификатор курьера
        /// </summary>
        [DataMember]
        public int? NewCarrierId { get; set; }

        [DataMember]
        public Dictionary<string, string> NewSettings { get; set; }
    }
}
