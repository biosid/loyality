namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using API.Entities;

    public class PartnerProductCategory
    {
        /// <summary>
        /// Идентификатор партнёра, источника категории
        /// </summary>
        public int PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор категории по справочнику партнёра
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор родительской категории по справочнику партнёра
        /// </summary>
        public string ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Имя категории
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Путь категории
        /// </summary>
        public string NamePath
        {
            get;
            set;
        }

        /// <summary>
        /// Статус категории
        /// </summary>
        public ProductCategoryStatuses Status
        {
            get;
            set;
        }
    }
}