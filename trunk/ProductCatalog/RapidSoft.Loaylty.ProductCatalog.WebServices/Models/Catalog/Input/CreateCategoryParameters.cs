namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CreateCategoryParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор родительской категории.
        /// </summary>
        [DataMember]
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Статус категории.
        /// </summary>
        [DataMember]
        public ProductCategoryStatuses Status { get; set; }

        /// <summary>
        /// Тип категории.
        /// </summary> 
        [DataMember]
        public ProductCategoryTypes Type { get; set; }

        /// <summary>
        /// Ссылка на динамическую категорию
        /// </summary>
        [DataMember]
        public string OnlineCategoryUrl { get; set; }

        /// <summary>
        /// Ссылка Notify
        /// </summary>
        [DataMember]
        public string NotifyOrderStatusUrl { get; set; }

        [DataMember]
        public int? OnlineCategoryPartnerId { get; set; }
    }
}