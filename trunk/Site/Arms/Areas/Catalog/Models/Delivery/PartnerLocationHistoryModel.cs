using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class PartnerLocationHistoryModel
    {
        public DateTime DateTime { get; set; }

        public string LocationId { get; set; }

        public string LocationName { get; set; }

        public PartnerLocationStatus OldStatus { get; set; }

        public PartnerLocationStatus NewStatus { get; set; }

        public string OldKladrCode { get; set; }

        public string NewKladrCode { get; set; }

        public string UserId { get; set; }

        public static PartnerLocationHistoryModel Map(PartnerLocationHistory original)
        {
            return new PartnerLocationHistoryModel
            {
                LocationName = original.Name,
                DateTime = original.DateTime,
                NewKladrCode = original.NewKladrCode,
                OldKladrCode = original.OldKladrCode,
                LocationId = "", // TODO: Прикрутить
                UserId = original.UserId,
                OldStatus = MapStatus(original.OldStatus),
                NewStatus = MapStatus(original.NewStatus)
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
                    return PartnerLocationStatus.WrongBinding;

                default:
                    return PartnerLocationStatus.NotBinded;
            }
        }
    }
}