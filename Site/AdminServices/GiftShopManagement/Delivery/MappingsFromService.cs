using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery
{
    internal static class MappingsFromService
    {
        public static ImportTaskStatus ToProductImportTaskStatus(ImportTaskStatuses original)
        {
            switch (original)
            {
                case ImportTaskStatuses.Canceled:
                    return ImportTaskStatus.Canceled;
                case ImportTaskStatuses.Completed:
                    return ImportTaskStatus.Completed;
                case ImportTaskStatuses.Error:
                    return ImportTaskStatus.Error;
                case ImportTaskStatuses.Loading:
                    return ImportTaskStatus.Loading;
                case ImportTaskStatuses.Waiting:
                    return ImportTaskStatus.Waiting;
                default:
                    return ImportTaskStatus.Error;
            }
        }

        public static DeliveryRatesImportTask ToDeliveryRatesImportTask(DeliveryRateImportTask original)
        {
            return new DeliveryRatesImportTask
            {
                Id = original.Id,
                CountFail = original.CountFail,
                CountSuccess = original.CountSuccess,
                CreationDateTime = original.InsertDateTime,
                EndDateTime = original.EndDateTime,
                StartDateTime = original.StartDateTime,
                FileUrl = original.FileUrl,
                PartnerId = original.PartnerId,
                UserId = original.InsertedUserId,
                Status = ToProductImportTaskStatus(original.Status)
            };
        }

        public static PartnerLocationStatus ToPartnerLocationBindingStatus(DeliveryLocationStatus original)
        {
            switch (original)
            {
                case DeliveryLocationStatus.CorrectBinded:
                    return PartnerLocationStatus.Binded;
                case DeliveryLocationStatus.NotBinded:
                    return PartnerLocationStatus.NotBinded;
                case DeliveryLocationStatus.NotCorrectKladr:
                    return PartnerLocationStatus.UnknownKladr;
                case DeliveryLocationStatus.KladrDuplication:
                    return PartnerLocationStatus.DuplicateKladr;
                default:
                    return PartnerLocationStatus.NotBinded;
            }
        }

        public static PartnerLocation ToPartnerLocationBinding(DeliveryLocation original)
        {
            return new PartnerLocation
            {
                Id = original.Id,
                CreationDateTime = original.InsertDateTime,
                KladrCode = original.Kladr,
                ExternalLocationId = original.ExternalLocationId,
                LocationName = original.LocationName,
                LastUpdateDateTime = original.UpdateDateTime,
                LastUpdateUserId = original.UpdateUserId,
                PartnerId = original.PartnerId,
                Status = ToPartnerLocationBindingStatus(original.Status)
            };
        }

        public static PartnerLocationHistory ToPartnerLocationHistory(DeliveryLocationHistory original)
        {
            return new PartnerLocationHistory
            {
                Id = original.Id,
                PartnerId = original.PartnerId,
                Name = original.LocationName,
                DateTime = original.UpdateDateTime,
                NewKladrCode = original.NewKladr,
                OldKladrCode = original.OldKladr,
                NewStatus = ToPartnerLocationBindingStatus(original.NewStatus),
                OldStatus = ToPartnerLocationBindingStatus(original.OldStatus),
                UserId = original.UpdateUserId,
                OldExternalId = original.OldExternaLocationlId,
                NewExternalId = original.NewExternaLocationlId
            };
        }
    }
}
