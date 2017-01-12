namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    using System;
    using System.Configuration;

    using Quartz;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;

    [DisallowConcurrentExecution]
    public class ImportYmlJob : ImportJobBase
    {
        private static readonly string UserId = ConfigurationManager.AppSettings["VtbSystemUserName"];
        private readonly ILog log = LogManager.GetLogger(typeof (ImportYmlJob));

        public override void Execute(IJobExecutionContext context)
        {
            var logEntry = StartInteraction.With("Partner").For("LoadYml");

            try
            {
                var provider = this.GetCatalogAdminServiceProvider();

                var dataMap = context.JobDetail.JobDataMap;

                var partnerId = dataMap.GetIntValue(DataKeys.PartnerId);
                var remoteFileUrl = dataMap.GetAsString(DataKeys.RemoteFileUrl);
                var localFilePath = dataMap.GetAsString(DataKeys.LocalFilePath);
                var localFileUrl = dataMap.GetAsString(DataKeys.LocalFileUrl);

                logEntry.Info["PartnerId"] = partnerId;
                logEntry.Info["RemoteFileUrl"] = remoteFileUrl;

                this.LoadFile(remoteFileUrl, localFilePath);

                var result = provider.ImportProductsFromYmlHttp(partnerId, localFileUrl, UserId);

                if (!result.Success)
                {
                    var mess = "Ошибка формирования задачи импорта в компоненте \"RapidSoft.Loaylty.ProductCatalog\": "
                               + result.ResultDescription;

                    logEntry.Failed(mess, null);

                    throw RestartHelper.GetRestartException(context, mess);
                }

                logEntry.Info["ImportTaskId"] = result.TaskId;
                logEntry.Succeeded();
            }
            catch (JobExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logEntry.Failed("ошибка взаимодействия", ex);
                throw RestartHelper.GetRestartException(context, exception: ex);
            }
            finally
            {
                logEntry.FinishAndWrite(log);
            }
        }
    }
}
