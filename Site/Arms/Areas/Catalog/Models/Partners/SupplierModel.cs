using Vtb24.Arms.Helpers;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class SupplierModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string TrustLevel { get; set; }

        public string Description { get; set; }

        public bool Disabled { get; set; }

        public static SupplierModel Map(Supplier supplier)
        {
            return new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Type = supplier.Type.Map().EnumDescription(),
                Status = supplier.Status.Map().EnumDescription(),
                TrustLevel = supplier.TrustLevel.Map().EnumDescription(),
                Description = supplier.Description,
                Disabled = supplier.Status == PartnerStatus.Disabled
            };
        }
    }
}
