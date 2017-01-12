using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class PartnerLocationModel
    {
        public int Id { get; set; }

        public string LocationId { get; set; }

        public string LocationName { get; set; }

        public PartnerLocationStatus Status { get; set; }

        public string KladrCode { get; set; }

        public static PartnerLocationModel Map(PartnerLocation original)
        {
            return new PartnerLocationModel
            {
                Id = original.Id,
                LocationName = original.LocationName,
                LocationId = original.ExternalLocationId,
                KladrCode = original.KladrCode,
                Status = MapStatus(original.Status)
            };
        }

        private static PartnerLocationStatus MapStatus(AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus original)
        {
            switch (original)
            {
                case AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus.Binded:
                    return PartnerLocationStatus.Binded;

                case AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus.NotBinded:
                    return PartnerLocationStatus.NotBinded;

                case AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus.UnknownKladr:
                case AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus.DuplicateKladr:
                    return PartnerLocationStatus.WrongBinding;

                default:
                    return PartnerLocationStatus.NotBinded;
            }
        }
    }
}