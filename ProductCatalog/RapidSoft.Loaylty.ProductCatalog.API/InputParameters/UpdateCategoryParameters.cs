namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using Entities;

    public class UpdateCategoryParameters
    {
        /// <summary>
        /// Идентификатор клиента обновляющего категорию.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор обновляемой категории.
        /// </summary>
        public int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string NewName
        {
            get;
            set;
        }

        /// <summary>
        /// Ссылка на динамическую категорию
        /// </summary>
        public string NewOnlineCategoryUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Notify Ссылка 
        /// </summary>
        public string NewNotifyOrderStatusUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Статус категории.
        /// </summary>
        public ProductCategoryStatuses NewStatus
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