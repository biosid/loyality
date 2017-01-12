using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks
{
    internal interface IImportTasks
    {
        ProductImportTaskResult GetProductsImportsHistory(int supplierId, PagingSettings paging);
    }
}
