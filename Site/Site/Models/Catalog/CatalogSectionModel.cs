namespace Vtb24.Site.Models.Catalog
{
    public class CatalogSectionModel : CatalogCategoryModel
    {
        public string IconName { get; set; }

        public CatalogRubricModel[][] RubricsColumns { get; set; }
    }
}
