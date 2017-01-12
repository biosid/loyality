using System;
using System.IO;
using System.Linq;
using System.Threading;
using Quartz;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;

namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    [DisallowConcurrentExecution]
    public class ClearDeletedGiftsFilesJob : JobBase, IInterruptableJob
    {
        private readonly ILog log = LogManager.GetLogger(typeof (ClearDeletedGiftsFilesJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var provider = GetCatalogAdminServiceProvider();

                var filesystem = GetFileSystem();

                var basePath = PartnerConnectionsConfig.GiftFilesPath;

                var productsDirs = filesystem
                                    .EnumerateDirectories(basePath)
                                    .Select(p => p.Replace(basePath, ""))
                                    .ToArray();

                var pageSize = 10;
                
                var i = 0;
                var batchHasRecords = true;
                
                while (batchHasRecords)
                {
                    var productsDirsBatch = productsDirs.Skip(i * pageSize).Take(pageSize).ToArray();

                    var result = provider.SearchAllProducts(productsIds: productsDirsBatch);

                    if (!result.Success)
                    {
                        var mess = "Ошибка формирования задачи удаления файлов продуктов, отсутствующих в \"RapidSoft.Loyalty.ProductCatalog\": "
                                   + result.ResultDescription;
                        throw RestartHelper.GetRestartException(context, mess);
                    }

                    var productsIds = result.Products.Select(p => SanitizeFilename(p.ProductId)).ToArray();

                    var deletedProductsDirsBatch = productsDirsBatch
                                                    .Except(productsIds)
                                                    .Select(p => Path.Combine(basePath, p))
                                                    .ToArray();

                    foreach (var deletedProductDirsPath in deletedProductsDirsBatch)
                    {
                        filesystem.DeleteDirectory(deletedProductDirsPath, true);
                    }

                    i++;
                    batchHasRecords = productsDirsBatch.Length > 0;
                }
            }
            catch (JobExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка обработки сообщения", ex);

                throw RestartHelper.GetRestartException(context, exception: ex);
            }
        }

        private static string SanitizeFilename(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '_'));
        }

        public void Interrupt()
        {
            log.Info("Прерывание выполнения");
            Thread.CurrentThread.Abort();
        }
    }
}
