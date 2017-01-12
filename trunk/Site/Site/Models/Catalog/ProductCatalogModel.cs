namespace Vtb24.Site.Models.Catalog
{
    public class ProductCatalogModel : BaseCatalogListingModel
    {
        public CatalogSectionModel[] CatalogSections { get; set; }

        public string[] Keywords { get; set; }
    }
}