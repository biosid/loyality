using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Models.Catalog
{
    public class GuestBankProductsModel
    {
        public CatalogSectionModel[] CatalogSections { get; set; }

        public BreadCrumbModel[] BreadCrumbs { get; set; }
    }
}
