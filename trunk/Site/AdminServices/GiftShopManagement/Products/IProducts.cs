using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products
{
    internal interface IProducts
    {
        ProductsSearchResult SearchProducts(ProductsSearchCriteria criteria, PagingSettings paging);

        string CreateProduct(Product product);

        void UpdateProduct(Product product);

        void ImportProductsFromYml(int supplierId, string fullPathName);

        void ModerateProducts(ModerateProductsOptions options);

        void DeleteProducts(string[] ids);

        void MoveProducts(MoveProductsOptions options);

        void ActivateProducts(ActivateProductsOptions options);

        void SetProductsSegments(SetProductsSegmentsOptions options);

        void RecommendProducts(RecommendProductsOptions options);
    }
}
