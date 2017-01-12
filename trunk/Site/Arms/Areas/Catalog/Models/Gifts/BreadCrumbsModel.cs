using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class BreadCrumbsModel
    {
        public CategoryItemModel[] BreadCrumbs { get; set; }

        public GiftsQueryModel Query { get; set; }
    }
}
