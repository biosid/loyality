using Vtb24.Site.Services.GiftShop.Model;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models
{
    public class CatalogProduct
    {
        public Product Product { get; set; }

        public ProductStatus ProductStatus { get; set; }

        public int ViewsCount { get; set; }

        public string Description { get; set; }

        public string[] Pictures { get; set; }

        public int PopularityRate { get; set; }

        public Parameter[] Parameters { get; set; }

        public class Parameter
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string Unit { get; set; }
        }
    }
}