namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Collections.Generic;

    using Entities;

    public class SearchPublicProductsParameters
    {
        public SortTypes? SortType { get; set; }

        public int[] ParentCategories { get; set; }

        public string SearchTerm { get; set; }

        public int[] PartnerIds { get; set; }

        public string[] Vendors { get; set; }

        public bool? ReturnEmptyVendorProducts { get; set; }

        public DateTime? MinInsertedDate { get; set; }

        public bool? IncludeSubCategory { get; set; }

        public ProductParam[] ProductParams { get; set; }

        public int? CountToTake { get; set; }

        public int? CountToSkip { get; set; }

        public Dictionary<string, string> ClientContext { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool? IsActionPrice { get; set; }
    }
}
