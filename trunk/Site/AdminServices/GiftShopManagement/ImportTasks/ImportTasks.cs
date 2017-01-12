using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.Outputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks
{
    // TODO: избавиться от этого сервиса
    internal class ImportTasks : IImportTasks
    {
        public ImportTasks(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public ProductImportTaskResult GetProductsImportsHistory(int supplierId, PagingSettings paging)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetImportTasksHistoryParameters
                {
                    UserId = _security.CurrentUser,

                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    PartnerId = supplierId,
                    CalcTotalCount = true
                };

                var response = service.GetProductCatalogImportTasksHistory(parameters);

                response.AssertSuccess();

                return new ProductImportTaskResult(response.Tasks.Select(MappingsFromService.ToProductImportTask),
                                                   response.TotalCount ?? 0,
                                                   paging);
            }
        }
    }
}
