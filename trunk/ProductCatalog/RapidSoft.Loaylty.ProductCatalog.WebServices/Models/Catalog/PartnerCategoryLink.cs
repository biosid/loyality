namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PartnerCategoryLink
    {
        /// <summary>
        /// Идентфикатор партнёра привязанной категории
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор привязанной категории рубрикатора
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Путь привязанной категории партнёра
        /// </summary>
        public PartnerCategoryPath[] Paths { get; set; }
    }
}
