using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.Catalog.Models.Shared.Helpers;

namespace Vtb24.Arms.Catalog.Models.PartnerCategories
{
    public class SupplierModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static SupplierModel Map(SupplierInfo supplier)
        {
            return new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.MapName()
            };
        }
    }
}
