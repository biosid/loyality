namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class SearchPublicProductsParameters
    {
        [DataMember]
        public int[] ParentCategories { get; set; }

        [DataMember]
        public bool? IncludeSubCategory { get; set; }

        [DataMember]
        public string SearchTerm { get; set; }

        [DataMember]
        public string[] Vendors { get; set; }

        [DataMember]
        public ProductParameterValue[] ProductParametersValues { get; set; }

        [DataMember]
        public DateTime? MinInsertedDate { get; set; }

        [DataMember]
        public decimal? MinPrice { get; set; }

        [DataMember]
        public decimal? MaxPrice { get; set; }

        [DataMember]
        public bool? IsActionPrice { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public ProductsSortTypes? Sort { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
