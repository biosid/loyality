namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System;

    public class PartnerProductCategoryLink
    {
        /// <summary>
        /// Идентификатор ссылки
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Идентфикатор партнёра привязанной категории
        /// </summary>
        public int PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор привязанной категории рубрикатора
        /// </summary>
        public int ProductCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Признак включения всех вложенных подкатегорий.
        /// </summary>
        public bool IncludeSubcategories
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

        public string InsertedUserId
        {
            get;
            set;
        }

        public DateTime InsertedDate
        {
            get;
            set;
        }
    }
}