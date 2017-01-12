using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class CreateCategoryParameters
    {
        /// <summary>
        /// Идентификатор клиента создающего категорию.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор родительской категории.
        /// </summary>
        public int? ParentCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Статус категории.
        /// </summary>
        public ProductCategoryStatuses Status
        {
            get;
            set;
        }

        /// <summary>
        /// Тип категории.
        /// </summary> 
        public ProductCategoryTypes Type
        {
            get;
            set;
        }

        /// <summary>
        /// Ссылка на динамическую категорию
        /// </summary>
        public string OnlineCategoryUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Ссылка Notify
        /// </summary>
        public string NotifyOrderStatusUrl
        {
            get;
            set;
        }

        public int? OnlineCategoryPartnerId
        {
            get;
            set;
        }
    }
}