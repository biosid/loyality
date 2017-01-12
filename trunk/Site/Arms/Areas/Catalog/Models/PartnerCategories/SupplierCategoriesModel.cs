namespace Vtb24.Arms.Catalog.Models.PartnerCategories
{
    public class SupplierCategoriesModel
    {
        public SupplierCategoryModel[] Categories { get; set; }

        public SupplierModel[] Suppliers { get; set; }

        public int SelectedSupplierId { get; set; }

        public SupplierCategoriesPermissionsModel Permissions { get; set; }
    }
}
