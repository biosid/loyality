namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal class Result
    {
        public Result(string errorDescription)
        {
            this.Product = null;
            this.IsValid = false;
            this.ErrorDescription = errorDescription;
        }

        public Result(Product product)
        {
            this.Product = product;
            this.IsValid = true;
            this.ErrorDescription = null;
        }

        public Product Product { get; private set; }

        public bool IsValid { get; private set; }

        public string ErrorDescription { get; private set; }
    }
}