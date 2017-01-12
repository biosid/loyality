namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs
{
    public class ModerateProductsOptions
    {
        public string[] ProductIds { get; set; }

        public ProductModerationStatus ModerationStatus { get; set; }
    }
}
