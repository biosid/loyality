namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using API.Entities;

    public class PartnerToCommonCategoryMap
    {
        public PartnerProductCategory PartnerCategory
        {
            get;
            set;
        }

        public ProductCategory ProductCategory
        {
            get;
            set;
        }
    }
}