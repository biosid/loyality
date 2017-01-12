using System.Collections.Generic;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models
{
    public class Category
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string CategoryPath { get; set; }

        public string OnlineCategoryUrl { get; set; }

        public string NotifyOrderStatusUrl { get; set; }

        public int? OnlineCategoryPartnerId { get; set; }

        public CategoryStatus Status { get; set; }

        public CategoryType Type { get; set; }

        public int Depth { get; set; }

        
        #region Метаданные

        public long? CountedProducts { get; set; }

        public int? CountedSubCategories { get; set; }

        #endregion


        #region Служебные поля для стабов

        internal IList<Category> SubCategories { get; set; }

        //internal IList<CatalogProduct> Products { get; set; }

        #endregion
    }
}