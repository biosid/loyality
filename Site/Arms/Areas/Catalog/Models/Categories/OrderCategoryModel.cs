using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class OrderCategoryModel
    {
        public int Id { get; set; }

        public int TargetId { get; set; }

        public MoveType Type { get; set; }

        public MoveCategoryOptions Map()
        {
            return new MoveCategoryOptions
            {
                CategoryId = Id,
                ReferenceCategoryId = TargetId,
                MoveOptions = MapMoveOptions()
            };
        }

        private MoveOptions MapMoveOptions()
        {
            switch (Type)
            {
                case MoveType.Before:
                    return MoveOptions.Before;
                default:
                    return MoveOptions.After;
            }   
        }

        public enum MoveType
        {
            Before,
            After
        }
    }
}