using System;
using System.Collections.Generic;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models
{
    [Serializable]
    public class CatalogCategory
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string CategoryPath { get; set; }

        public int Depth { get; set; }


        #region  Служебные поля для стабов

        internal IList<CatalogCategory> SubCategories { get; set; }

        internal IList<CatalogProduct> Products { get; set; } 

        #endregion


        #region Метаданные

        /// <summary>
        /// Кол-во вложенных категорий
        /// </summary>
        public int SubCategoriesCount { get; set; }

        /// <summary>
        /// Кол-во товаров в категории
        /// </summary>
        public long ProductsCount { get; set; }

        /// <summary>
        /// Тип категории - онлайн или статическая
        /// </summary>
        public CategoryType CategoryType { get; set; }

        /// <summary>
        /// Url для загрузки партнера в iframe
        /// </summary>
        public string OnlineCategoryUrl { get; set; }

        /// <summary>
        /// Url для нотификации партнера об изменении статуса заказа
        /// </summary>
        public string OnlineCategoryReturnUrl { get; set; }

        public int? OnlineCategoryPartnerId { get; set; }
        
        #endregion
    }
}