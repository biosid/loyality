using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery
{
    internal static class MappingsToService
    {
        public static DeliveryLocationStatus FromPartnerLocationBindingStatus(PartnerLocationStatus original)
        {
            switch (original)
            {
                case PartnerLocationStatus.Binded:
                    return DeliveryLocationStatus.CorrectBinded;
                case PartnerLocationStatus.NotBinded:
                    return DeliveryLocationStatus.NotBinded;
                case PartnerLocationStatus.UnknownKladr:
                    return DeliveryLocationStatus.NotCorrectKladr;
                case PartnerLocationStatus.DuplicateKladr:
                    return DeliveryLocationStatus.KladrDuplication;
                default:
                    return DeliveryLocationStatus.NotBinded;
            }
        }
    }
}
