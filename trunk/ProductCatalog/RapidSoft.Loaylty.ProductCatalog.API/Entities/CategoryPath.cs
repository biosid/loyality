namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public class CategoryPath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPath"/> class.
        /// </summary>
        public CategoryPath()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPath"/> class.
        /// </summary>
        /// <param name="includeSubcategories">
        /// The include subcategories.
        /// </param>
        /// <param name="namePath">
        /// The name path.
        /// </param>
        public CategoryPath(bool includeSubcategories, string namePath)
        {
            this.IncludeSubcategories = includeSubcategories;
            this.NamePath = namePath;
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
    }
}