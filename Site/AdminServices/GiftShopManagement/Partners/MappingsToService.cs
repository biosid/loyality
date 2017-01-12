using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using PartnerStatus = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.PartnerStatus;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners
{
    internal static class MappingsToService
    {
        public static CatalogAdminService.PartnerStatus ToPartnerStatus(PartnerStatus original)
        {
            switch (original)
            {
                case PartnerStatus.Enabled:
                    return CatalogAdminService.PartnerStatus.Active;
                case PartnerStatus.Disabled:
                    return CatalogAdminService.PartnerStatus.NotActive;
            }
            return CatalogAdminService.PartnerStatus.NotActive;
        }

        public static PartnerThrustLevel ToPartnerTrustLevel(SupplierTrustLevel original)
        {
            switch (original)
            {
                case SupplierTrustLevel.Low:
                    return PartnerThrustLevel.Low;
                case SupplierTrustLevel.Middle:
                    return PartnerThrustLevel.Middle;
                case SupplierTrustLevel.High:
                    return PartnerThrustLevel.High;
            }
            return PartnerThrustLevel.Low;
        }

        public static CatalogAdminService.PartnerType ToPartnerType(SupplierType original)
        {
            switch (original)
            {
                case SupplierType.Direct:
                    return CatalogAdminService.PartnerType.Direct;
                case SupplierType.Offline:
                    return CatalogAdminService.PartnerType.Offline;
                case SupplierType.Online:
                    return CatalogAdminService.PartnerType.Online;
            }
            return CatalogAdminService.PartnerType.Direct;
        }
    }
}
