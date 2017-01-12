namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;

    using API.Entities;
    using API.InputParameters;

    public class SearchProductsParameters
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

        public decimal? MinRecommendedPrice { get; set; }

        public decimal? MaxRecommendedPrice { get; set; }

        public int? CategoryIdToRecommendFor { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool? IsActionPrice { get; set; }

        public PopularProductTypes? PopularProductType { get; set; }
    }
}