using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery
{
    internal interface IDelivery
    {
        DeliveryRatesImportTaskResult GetDeliveryRatesImportsHistory(int partnerId, PagingSettings paging);

        PartnerLocationsBindingsResult GetDeliveryLocations(GetPartnerLocationsOptions options,
                                                                   PagingSettings paging);

        PartnerLocationsHistoryResult GetDeliveryLocationsHistory(int partnerId, PagingSettings paging);

        void SetDeliveryLocationKladr(int locationId, string kladr);

        void ResetDeliveryLocationKladr(int locationId);
    }
}
