using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Models.Catalog
{
    public class BankProductsModel
    {
        public CatalogSectionModel[] CatalogSections { get; set; }

        public BreadCrumbModel[] BreadCrumbs { get; set; }

        public ListBankProductModel[] Products { get; set; }

        public bool IsUserActivated { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }
    }
}
