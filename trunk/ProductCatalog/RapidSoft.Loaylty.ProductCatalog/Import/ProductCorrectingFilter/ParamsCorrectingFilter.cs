namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal class ParamsCorrectingFilter : IProductCorrectingFilter
    {
        private readonly ParamsProcessTypes paramsProcessType;

        public ParamsCorrectingFilter(ParamsProcessTypes paramsProcessType)
        {
            this.paramsProcessType = paramsProcessType;
        }

        public Result Execute(Product product)
        {
            if (product == null)
            {
                const string MessFormat = "Null не допустимое значение для продукта";
                return new Result(MessFormat);
            }

            switch (this.paramsProcessType)
            {
                case ParamsProcessTypes.NotAcceptParamDuplicate:
                    {
                        var productParams = product.Param;

                        if (productParams == null)
                        {
                            return new Result(product);
                        }

                        productParams = productParams.Distinct().ToArray();

                        var hasError = productParams.GroupBy(x => x.Name).FirstOrDefault(x => x.Count() > 1);

                        if (hasError != null)
                        {
                            const string MessFormat = "Продукт {0} имеет несколько параметров с именем {1}";
                            return new Result(string.Format(MessFormat, product.PartnerProductId, hasError.Key));
                        }

                        product.Param = productParams;
                        return new Result(product);
                    }

                default:
                    {
                        return new Result(product);
                    }
            }
        }
    }
}