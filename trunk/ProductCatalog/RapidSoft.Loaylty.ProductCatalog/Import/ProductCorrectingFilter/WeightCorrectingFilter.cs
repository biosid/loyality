namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal class WeightCorrectingFilter : IProductCorrectingFilter
    {
        private readonly WeightProcessTypes weightProcessTypes;

        public WeightCorrectingFilter(WeightProcessTypes weightProcessTypes)
        {
            this.weightProcessTypes = weightProcessTypes;
        }

        public Result Execute(Product product)
        {
            if (product == null)
            {
                const string MessFormat = "Null не допустимое значение для продукта";
                return new Result(MessFormat);
            }

            switch (this.weightProcessTypes)
            {
                case WeightProcessTypes.DefaultWeight:
                    {
                        product.Weight = product.Weight ?? 0;
                        return new Result(product);
                    }

                case WeightProcessTypes.WeightRequired:
                    {
                        if (!product.Weight.HasValue)
                        {
                            const string MessFormat = "Продукт {0} не содержит данные о весе";
                            return new Result(string.Format(MessFormat, product.PartnerProductId));
                        }

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