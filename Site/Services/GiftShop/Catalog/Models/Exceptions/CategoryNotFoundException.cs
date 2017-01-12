namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions
{
    public class CategoryNotFoundException: CatalogServiceException
    {
        public CategoryNotFoundException(int resultCode, string codeDescription) :
            base(resultCode, codeDescription)
        {
        }
         
    }
}