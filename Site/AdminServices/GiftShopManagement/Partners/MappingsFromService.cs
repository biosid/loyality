using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Partner = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.Partner;
using PartnerInfo = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.PartnerInfo;
using PartnerStatus = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.PartnerStatus;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners
{

    internal static class MappingsFromService
    {
        public static PartnerStatus ToPartnerStatus(CatalogAdminService.PartnerStatus original)
        {
            switch (original)
            {
                case CatalogAdminService.PartnerStatus.Active:
                    return PartnerStatus.Enabled;
                case CatalogAdminService.PartnerStatus.NotActive:
                    return PartnerStatus.Disabled;
            }
            return PartnerStatus.Disabled;
        }

        public static SupplierTrustLevel ToSupplierTrustLevel(PartnerThrustLevel original)
        {
            switch (original)
            {
                case PartnerThrustLevel.Low:
                    return SupplierTrustLevel.Low;
                case PartnerThrustLevel.Middle:
                    return SupplierTrustLevel.Middle;
                case PartnerThrustLevel.High:
                    return SupplierTrustLevel.High;
            }
            return SupplierTrustLevel.Low;
        }

        public static SupplierType ToSupplierType(PartnerType original)
        {
            switch (original)
            {
                case PartnerType.Direct:
                    return SupplierType.Direct;
                case PartnerType.Offline:
                    return SupplierType.Offline;
                case PartnerType.Online:
                    return SupplierType.Online;
            }
            return SupplierType.Direct;
        }

        public static SupplierInfo ToSupplierInfo(CatalogAdminService.PartnerInfo original)
        {
            return new SupplierInfo
            {
                Id = original.Id,
                Name = original.Name,
                Type = ToSupplierType(original.Type),
                Status = ToPartnerStatus(original.Status)
            };
        }

        public static Supplier ToSupplier(CatalogAdminService.Partner original)
        {
            return new Supplier
            {
                Id = original.Id,
                Name = original.Name,
                Description = original.Description,
                Status = ToPartnerStatus(original.Status),
                TrustLevel = ToSupplierTrustLevel(original.ThrustLevel),
                CarrierId = original.CarrierId,
                Type = ToSupplierType(original.Type),
                Settings = original.Settings
            };
        }

        public static CarrierInfo ToCarrierInfo(CatalogAdminService.PartnerInfo original)
        {
            return new CarrierInfo
            {
                Id = original.Id,
                Name = original.Name,
                Status = ToPartnerStatus(original.Status)
            };
        }

        public static Carrier ToCarrier(CatalogAdminService.Partner original)
        {
            return new Carrier
            {
                Id = original.Id,
                Name = original.Name,
                Description = original.Description,
                Status = ToPartnerStatus(original.Status),
                Settings = original.Settings
            };
        }

        public static PartnerInfo ToPartnerInfo(CatalogAdminService.PartnerInfo original)
        {
            return original.IsCarrier
                       ? (PartnerInfo) ToCarrierInfo(original)
                       : ToSupplierInfo(original);
        }

        public static Partner ToPartner(CatalogAdminService.Partner original)
        {
            return original.IsCarrier
                       ? (Partner) ToCarrier(original)
                       : ToSupplier(original);
        }
    }
}
