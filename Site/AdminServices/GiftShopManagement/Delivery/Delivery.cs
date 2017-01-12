using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery
{
    internal class Delivery : IDelivery
    {
        public Delivery(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public DeliveryRatesImportTaskResult GetDeliveryRatesImportsHistory(int partnerId, PagingSettings paging)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetImportTasksHistoryParameters
                {
                    UserId = _security.CurrentUser,
                    
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    PartnerId = partnerId,
                    CalcTotalCount = true
                };

                var response = service.GetDeliveryRateImportTasksHistory(parameters);

                response.AssertSuccess();

                return new DeliveryRatesImportTaskResult(
                        response.Tasks.Select(MappingsFromService.ToDeliveryRatesImportTask),
                        response.TotalCount ?? 0,
                        paging);
            }
        }

        public PartnerLocationsBindingsResult GetDeliveryLocations(GetPartnerLocationsOptions options, PagingSettings paging)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetDeliveryLocationsParameters
                {
                    StatusFilters = options.Statuses != null ? 
                                        options.Statuses.Select(MappingsToService.FromPartnerLocationBindingStatus).ToArray() : 
                                        null,
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    PartnerId = options.PartnerId,
                    SearchTerm = options.SearchTerm,
                    HasRates = true,
                    CalcTotalCount = true
                };

                var response = service.GetDeliveryLocations(parameters, _security.CurrentUser);

                response.AssertSuccess();

                return new PartnerLocationsBindingsResult(
                    response.DeliveryLocations.Select(MappingsFromService.ToPartnerLocationBinding),
                    response.TotalCount ?? 0,
                    paging);
            }
        }

        public PartnerLocationsHistoryResult GetDeliveryLocationsHistory(int partnerId, PagingSettings paging)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetDeliveryLocationHistoryParameters
                {
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    PartnerId = partnerId,
                    CalcTotalCount = true                   
                };

                var response = service.GetDeliveryLocationHistory(parameters, _security.CurrentUser);

                response.AssertSuccess();

                return new PartnerLocationsHistoryResult(
                    response.DeliveryLocationHistory.Select(MappingsFromService.ToPartnerLocationHistory),
                    response.TotalCount ?? 0,
                    paging);
            }
        }

        public void SetDeliveryLocationKladr(int locationId, string kladr)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.SetDeliveryLocationKladr(locationId, kladr, _security.CurrentUser);

                response.AssertSuccess();
            }
        }

        public void ResetDeliveryLocationKladr(int locationId)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.ResetDeliveryLocation(locationId, _security.CurrentUser);

                response.AssertSuccess();
            }
        }
    }
}
