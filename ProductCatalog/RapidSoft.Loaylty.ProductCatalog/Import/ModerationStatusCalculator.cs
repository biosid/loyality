namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal abstract class ModerationStatusCalculator : IModerationStatusCalculator
    {
        private readonly IList<Product> existsProduct;

        protected ModerationStatusCalculator(IList<Product> existsProduct)
        {
            this.existsProduct = existsProduct;
        }

        public abstract ProductModerationStatuses CalcModerationStatus(Product newProduct);

        protected Product GetExistAs(Product newProduct)
        {
            var retVal =
                this.existsProduct.SingleOrDefault(
                    x => x.PartnerId == newProduct.PartnerId && x.PartnerProductId == newProduct.PartnerProductId);
            return retVal;
        }
    }
}