using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models;
using ProductImportTask = Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.ProductImportTask;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks
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

        public static ProductImportTask ToProductImportTask(CatalogAdminService.ProductImportTask original)
        {
            return new ProductImportTask
            {
                Id = original.Id,
                CountFail = original.CountFail,
                CountSuccess = original.CountSuccess,
                CreationDateTime = original.InsertedDate,
                EndDateTime = original.EndDateTime,
                StartDateTime = original.StartDateTime,
                FileUrl = original.FileUrl,
                SupplierId = original.PartnerId,
                UserId = original.InsertedUserId,
                Status = ToProductImportTaskStatus(original.Status)
            };
        }
    }
}
