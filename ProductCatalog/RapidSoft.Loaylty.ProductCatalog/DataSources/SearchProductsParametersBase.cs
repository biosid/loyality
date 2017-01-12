namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public abstract class SearchProductsParametersBase : IPagingParameters, IClientContextParameters
    {
        public SortTypes? SortType { get; set; }

        public int[] ParentCategories { get; set; }

        public string SearchTerm { get; set; }

        public int[] PartnerIds { get; set; }

        public string[] ProductIds { get; set; }

        public string[] Vendors { get; set; }

        public bool? ReturnEmptyVendorProducts { get; set; }

        public DateTime? MinInsertedDate { get; set; }

        public bool? IncludeSubCategory { get; set; }

        public string PartnerProductId { get; set; }

        public ProductParam[] ProductParams { get; set; }

        public int? CountToTake { get; set; }

        public int? CountToSkip { get; set; }

        public bool? CalcTotalCount { get; set; }

        public Dictionary<string, string> ClientContext { get; set; }

        public int? ProductsQuantity { get; set; }
    }
}