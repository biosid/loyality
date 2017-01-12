using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class MoveCategoryModel
    {
        public int Id { get; set; }

        public int? TargetId { get; set; }


        public MoveCategoryOptions Map()
        {
            return new MoveCategoryOptions
            {
                CategoryId = Id,
                ReferenceCategoryId = TargetId,
                MoveOptions = MoveOptions.Append
            };
        }
    }
}