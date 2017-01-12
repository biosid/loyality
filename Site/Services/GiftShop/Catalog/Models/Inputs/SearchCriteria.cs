using System;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs
{
    public class SearchCriteria
    {
        public string SearchTerm { get; set; }

        public CatalogSort Sorting { get; set; }
        
        public bool ExcludeSubCategories { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool IsActionPrice { get; set; }

        public int[] Categories { get; set; }

        public string[] Vendors { get; set; }

        public bool ReturnEmptyVendorProducts { get; set; }

        public int[] CatalogPartners { get; set; }

        public DateTime? MinAddedToCatalogDate { get; set; }

        public CatalogParameterCriteria[] Parameters { get; set; }
    }
}
