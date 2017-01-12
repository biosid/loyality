using Vtb24.Arms.Catalog.Models.PartnerCategories;
using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftsModel
    {
        public SupplierModel[] Suppliers { get; set; }

        public GiftsQueryModel Query { get; set; }

        public CategoryItemModel[] CategoryItems { get; set; }

        public BreadCrumbsModel BreadCrumbs { get; set; }

        public GiftModel[] Gifts { get; set; }

        public int TotalPages { get; set; }

        public bool DisallowCreate { get; set; }

        public string DisallowCreateMessage { get; set; }

        public SegmentModel[] Segments { get; set; }

        public GiftsPermissionsModel Permissions { get; set; }
    }
}
