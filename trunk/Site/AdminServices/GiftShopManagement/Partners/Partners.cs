using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.Infrastructure;
using Partner = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.Partner;
using PartnerInfo = Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models.PartnerInfo;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners
{
    internal class Partners : IPartners
    {
        public Partners(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        #region получение краткой информации о партнерах

        public PartnerInfo[] GetPartnersInfo()
        {
            return GetServicePartnersInfo().Select(MappingsFromService.ToPartnerInfo)
                                           .ToArray();
        }

        public SupplierInfo[] GetSuppliersInfo()
        {
            return GetServicePartnersInfo().Where(p => !p.IsCarrier)
                                           .Select(MappingsFromService.ToSupplierInfo)
                                           .ToArray();
        }

        public CarrierInfo[] GetCarriersInfo()
        {
            return GetServicePartnersInfo().Where(p => p.IsCarrier)
                                           .Select(MappingsFromService.ToCarrierInfo)
                                           .ToArray();
        }

        public PartnerInfo[] GetUserPartnersInfo()
        {
            return GetServiceUserPartnersInfo().Select(MappingsFromService.ToPartnerInfo)
                                               .ToArray();
        }

        public SupplierInfo[] GetUserSuppliersInfo()
        {
            return GetServiceUserPartnersInfo().Where(p => !p.IsCarrier)
                                               .Select(MappingsFromService.ToSupplierInfo)
                                               .ToArray();
        }

        public CarrierInfo[] GetUserCarriersInfo()
        {
            return GetServiceUserPartnersInfo().Where(p => p.IsCarrier)
                                               .Select(MappingsFromService.ToCarrierInfo)
                                               .ToArray();
        }

        public PartnerInfo GetPartnerInfoById(int id)
        {
            var partnerInfo = GetServicePartnerInfoById(id);

            return MappingsFromService.ToPartnerInfo(partnerInfo);
        }

        public SupplierInfo GetSupplierInfoById(int id)
        {
            var partnerInfo = GetServicePartnerInfoById(id);

            return !partnerInfo.IsCarrier
                       ? MappingsFromService.ToSupplierInfo(partnerInfo)
                       : null;
        }

        public CarrierInfo GetCarrierInfoById(int id)
        {
            var partnerInfo = GetServicePartnerInfoById(id);

            return partnerInfo.IsCarrier
                       ? MappingsFromService.ToCarrierInfo(partnerInfo)
                       : null;
        }

        public PartnerInfo GetUserPartnerInfoById(int id)
        {
            var ids = _security.CurrentPermissions.GetGranted<int>(PermissionKeys.Catalog_PartnerIds);

            return ids != null && (ids.Length == 0 || ids.Contains(id))
                       ? GetPartnerInfoById(id)
                       : null;
        }

        public SupplierInfo GetUserSupplierInfoById(int id)
        {
            var ids = _security.CurrentPermissions.GetGranted<int>(PermissionKeys.Catalog_PartnerIds);

            return ids != null && (ids.Length == 0 || ids.Contains(id))
                       ? GetSupplierInfoById(id)
                       : null;
        }

        public CarrierInfo GetUserCarrierInfoById(int id)
        {
            var ids = _security.CurrentPermissions.GetGranted<int>(PermissionKeys.Catalog_PartnerIds);

            return ids != null && (ids.Length == 0 || ids.Contains(id))
                       ? GetCarrierInfoById(id)
                       : null;
        }

        #endregion

        #region получение полной информации о партнерах

        public Partner[] GetPartners()
        {
            return GetServicePartners().Select(MappingsFromService.ToPartner).ToArray();
        }

        public Supplier[] GetSuppliers()
        {
            return GetServicePartners().Where(p => !p.IsCarrier)
                                       .Select(MappingsFromService.ToSupplier)
                                       .ToArray();
        }

        public Carrier[] GetCarriers()
        {
            return GetServicePartners().Where(p => p.IsCarrier)
                                       .Select(MappingsFromService.ToCarrier)
                                       .ToArray();
        }

        public Partner GetPartnerById(int id)
        {
            var partner = GetServicePartnerById(id);

            return MappingsFromService.ToPartner(partner);
        }

        public Supplier GetSupplierById(int id)
        {
            var partner = GetServicePartnerById(id);

            return !partner.IsCarrier
                       ? MappingsFromService.ToSupplier(partner)
                       : null;
        }

        public Carrier GetCarrierById(int id)
        {
            var partner = GetServicePartnerById(id);

            return partner.IsCarrier
                       ? MappingsFromService.ToCarrier(partner)
                       : null;
        }

        #endregion

        #region создание/обновление партнеров

        public int CreateSupplier(Supplier supplier)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new CreatePartnerParameters
                {
                    UserId = _security.CurrentUser,
                    Name = supplier.Name,
                    Description = supplier.Description,
                    Status = MappingsToService.ToPartnerStatus(supplier.Status),
                    Type = MappingsToService.ToPartnerType(supplier.Type),
                    ThrustLevel = MappingsToService.ToPartnerTrustLevel(supplier.TrustLevel),
                    CarrierId = supplier.CarrierId,
                    IsCarrier = false,
                    Settings = supplier.Settings
                };

                var response = service.CreatePartner(parameters);

                response.AssertSuccess();

                return response.Partner.Id;
            }
        }

        public int CreateCarrier(Carrier carrier)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new CreatePartnerParameters
                {
                    UserId = _security.CurrentUser,
                    Name = carrier.Name,
                    Description = carrier.Description,
                    Status = MappingsToService.ToPartnerStatus(carrier.Status),
                    Type = PartnerType.Direct,
                    ThrustLevel = PartnerThrustLevel.Low,
                    CarrierId = null,
                    IsCarrier = true,
                    Settings = carrier.Settings
                };

                var response = service.CreatePartner(parameters);

                response.AssertSuccess();

                return response.Partner.Id;
            }
        }

        public void UpdateSupplier(Supplier supplier)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new UpdatePartnerParameters
                {
                    Id = supplier.Id,
                    UpdatedUserId = _security.CurrentUser,
                    NewName = supplier.Name,
                    NewDescription = supplier.Description,
                    NewStatus = MappingsToService.ToPartnerStatus(supplier.Status),
                    NewType = MappingsToService.ToPartnerType(supplier.Type),
                    NewThrustLevel = MappingsToService.ToPartnerTrustLevel(supplier.TrustLevel),
                    NewCarrierId = supplier.CarrierId,
                    NewSettings = supplier.Settings
                };

                var response = service.UpdatePartner(parameters);

                response.AssertSuccess();
            }
        }

        public void UpdateCarrier(Carrier carrier)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new UpdatePartnerParameters
                {
                    Id = carrier.Id,
                    UpdatedUserId = _security.CurrentUser,
                    NewName = carrier.Name,
                    NewDescription = carrier.Description,
                    NewStatus = MappingsToService.ToPartnerStatus(carrier.Status),
                    NewType = PartnerType.Direct,
                    NewThrustLevel = PartnerThrustLevel.Low,
                    NewCarrierId = null,
                    NewSettings = carrier.Settings
                };

                var response = service.UpdatePartner(parameters);

                response.AssertSuccess();
            }
        }

        #endregion

        #region загрузка матрицы стоимости доставки

        public void ImportDeliveryRatesFromHttp(int partnerId, string fileUrl)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.ImportDeliveryRatesFromHttp(partnerId, fileUrl, _security.CurrentUser);

                response.AssertSuccess();
            }
        }

        #endregion

        private IEnumerable<CatalogAdminService.PartnerInfo> GetServicePartnersInfo()
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartnersInfo(new int[0], _security.CurrentUser);

                response.AssertSuccess();

                return response.PartnersInfo;
            }
        }

        private IEnumerable<CatalogAdminService.PartnerInfo> GetServiceUserPartnersInfo()
        {
            var ids = _security.CurrentPermissions.GetGranted<int>(PermissionKeys.Catalog_PartnerIds);
            if (ids == null)
            {
                return Enumerable.Empty<CatalogAdminService.PartnerInfo>();
            }

            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartnersInfo(ids, _security.CurrentUser);

                response.AssertSuccess();

                return response.PartnersInfo;
            }
        }

        private CatalogAdminService.PartnerInfo GetServicePartnerInfoById(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartnerInfoById(id, _security.CurrentUser);

                response.AssertSuccess();

                return response.PartnerInfo;
            }
        }

        private IEnumerable<CatalogAdminService.Partner> GetServicePartners()
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartners(new int[0], _security.CurrentUser);

                response.AssertSuccess();

                return response.Partners;
            }
        }

        private CatalogAdminService.Partner GetServicePartnerById(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartnerById(id, _security.CurrentUser);

                response.AssertSuccess();

                return response.Partner;
            }
        }
    }


}