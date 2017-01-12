using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetAllSubCategoriesParameters
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
        /// Количество пропускаемых категорий
        /// </summary>
        [DataMember]
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Количество отбираемых категорий на страницу
        /// </summary>
        [DataMember]
        public int? CountToTake { get; set; }

        /// <summary>
        /// Считать ли общее количество найденных категорий
        /// </summary>
        [DataMember]
        public bool? CalcTotalCount { get; set; }

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

        /// <summary>
        /// Идентификатор пользователя в системе безопасности
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
    }
}