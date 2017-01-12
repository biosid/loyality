namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class UpdateCategoryParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор обновляемой категории.
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        [DataMember]
        public string NewName { get; set; }

        /// <summary>
        /// Ссылка на динамическую категорию
        /// </summary>
        [DataMember]
        public string NewOnlineCategoryUrl { get; set; }

        /// <summary>
        /// Notify Ссылка 
        /// </summary>
        [DataMember]
        public string NewNotifyOrderStatusUrl { get; set; }

        /// <summary>
        /// Статус категории.
        /// </summary>
        [DataMember]
        public ProductCategoryStatuses NewStatus { get; set; }

        [DataMember]
        public int? OnlineCategoryPartnerId { get; set; }
    }
}
