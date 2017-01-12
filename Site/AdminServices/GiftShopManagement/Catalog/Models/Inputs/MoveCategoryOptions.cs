namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs
{
    public class MoveCategoryOptions
    {
        public int CategoryId { get; set; }

        public int? ReferenceCategoryId { get; set; }

        public MoveOptions MoveOptions { get; set; }
    }
}