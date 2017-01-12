namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs
{
    public class ActivateProductsOptions
    {
        public string[] ProductIds { get; set; }

        public ProductStatus Status { get; set; }
    }
}
