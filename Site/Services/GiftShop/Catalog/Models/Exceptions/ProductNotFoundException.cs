namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions
{
    public class ProductNotFoundException : CatalogServiceException
    {
        public ProductNotFoundException(int resultCode, string codeDescription) :
            base(resultCode, codeDescription)
        {
        }
    }
}