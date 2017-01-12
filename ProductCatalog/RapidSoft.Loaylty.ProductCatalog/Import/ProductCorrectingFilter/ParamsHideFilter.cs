namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.YML;

    internal class ParamsHideFilter : IProductCorrectingFilter
    {
        private static readonly string[] ParamNamesToHide = new[]
        {
            YmlSettings.YmlParamIsDeliveredByEmailName
        };

        public Result Execute(Product product)
        {
            if (product == null)
            {
                const string MessFormat = "Null не допустимое значение для продукта";
                return new Result(MessFormat);
            }

            if (product.Param == null)
            {
                return new Result(product);
            }

            product.Param = product.Param.Where(p => !ParamNamesToHide.Contains(p.Name)).ToArray();

            return new Result(product);
        }
    }
}