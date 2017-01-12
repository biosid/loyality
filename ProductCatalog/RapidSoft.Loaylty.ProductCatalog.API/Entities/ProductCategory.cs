namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]    
    public class ProductCategory
    {
        /// <summary>
        /// Внутренний идентификатор категории
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор родительской категории
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }

        /// <summary>
        /// Идентификатор онлайн партнёра которому принадлежит категория
        /// </summary>
        [DataMember]
        public int? OnlineCategoryPartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Наименование категории
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Уникальный путь категории
        /// </summary>
        [DataMember]
        public string NamePath { get; set; }

        /// <summary>
        /// Количество продуктов в категории
        /// </summary>
        [DataMember]
        public long ProductsCount { get; set; }

        /// <summary>
        /// Статус категории
        /// </summary>
        [DataMember]
        public ProductCategoryStatuses Status { get; set; }

        /// <summary>
        /// Количество под директорий
        /// </summary>
        [DataMember]
        public int SubCategoriesCount { get; set; }

       public List<ProductCategory> SubCategories { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        [DataMember]
        public ProductCategoryTypes Type
        {
            get;
            set;
        }

        /// <summary>
        /// Ссылка на категорию онлайн партнёра
        /// </summary>
        [DataMember]
        public string OnlineCategoryUrl
        {
            get;
            set;
        }

        /// <summary>
        /// notify url
        /// </summary>
        [DataMember]
        public string NotifyOrderStatusUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь создавший категорию
        /// </summary>
        [DataMember]
        public string InsertedUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь обновивший категорию в последний раз
        /// </summary>
        public string UpdatedUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Дата последнего обновления категории
        /// </summary>
        public DateTime? UpdatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Дата создания категории
        /// </summary>
        public DateTime InsertedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Порядковый номер категории
        /// </summary>
        public int CatOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Уровень вложенности категории. 1 - для корневых категорий
        /// </summary>
        [DataMember]
        public int NestingLevel
        {
            get;
            set;
        }
    }
}
