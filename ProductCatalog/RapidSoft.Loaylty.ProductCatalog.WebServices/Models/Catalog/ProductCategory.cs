namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductCategory
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public int? OnlineCategoryPartnerId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NamePath { get; set; }

        [DataMember]
        public long ProductsCount { get; set; }

        [DataMember]
        public ProductCategoryStatuses Status { get; set; }

        [DataMember]
        public int SubCategoriesCount { get; set; }

        public ProductCategory[] SubCategories { get; set; }

        [DataMember]
        public ProductCategoryTypes Type { get; set; }

        [DataMember]
        public string OnlineCategoryUrl { get; set; }

        [DataMember]
        public string NotifyOrderStatusUrl { get; set; }

        [DataMember]
        public string InsertedUserId { get; set; }

        [DataMember]
        public string UpdatedUserId { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public DateTime InsertedDate { get; set; }

        [DataMember]
        public int CatOrder { get; set; }

        [DataMember]
        public int NestingLevel { get; set; }
    }
}
