using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Catalog
{
    public class ProductParamModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Unit { get; set; }

        public static ProductParamModel Map(CatalogProduct.Parameter original)
        {
            var parameter = new ProductParamModel
            {
                Name = original.Name,
                Value = original.Value,
                Unit = original.Unit
            };

            return parameter;
        }
    }
}