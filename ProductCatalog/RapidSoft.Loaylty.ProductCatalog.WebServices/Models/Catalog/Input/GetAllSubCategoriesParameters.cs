namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetAllSubCategoriesParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Фильтр по идентификатору родительской категории
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }

        /// <summary>
        /// Возвращать ли родительскую категорию 
        /// </summary>
        [DataMember]
        public bool IncludeParent { get; set; }

        /// <summary>
        /// Фильтр по уровню вложенности категорий
        /// </summary>
        [DataMember]
        public int? NestingLevel { get; set; }

        /// <summary>
        /// Фильтр по типу категории 
        /// </summary>
        [DataMember]
        public ProductCategoryTypes? Type { get; set; }

        /// <summary>
        /// Фильтр по статусу категории
        /// </summary>
        [DataMember]
        public ProductCategoryStatuses? Status { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
