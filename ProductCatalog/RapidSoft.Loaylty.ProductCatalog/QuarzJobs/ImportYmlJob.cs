namespace RapidSoft.Loaylty.ProductCatalog.QuarzJobs
{
    using System;
    using System.Globalization;

    using Quartz;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.Import;

    [GroupDisallowConcurrentExecution]
    public class ImportYmlJob : IJob
    {
        private const string JobGroupName = "ImportYml";

        private readonly ILog log = LogManager.GetLogger(typeof(ImportYmlJob));

        public static void AddJob(int taskId)
        {
            var scheduler = Helper.GetScheduler();

            var taskIdStr = taskId.ToString(CultureInfo.InvariantCulture);

            var job = JobBuilder.Create<ImportYmlJob>()
                .WithIdentity(taskIdStr, JobGroupName)
                .RequestRecovery(false)
                .Build();

            job.JobDataMap.Put(DataKeys.TaskId, taskId);

            var trigger = TriggerBuilder.Create()
                .WithIdentity(taskIdStr, JobGroupName)
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                log.Info("Начало обработки задачи на импорт xml-файла каталога продуктов");
                var dataMap = context.JobDetail.JobDataMap;

                var taskId = dataMap.GetIntValue(DataKeys.TaskId);

                log.DebugFormat("Импорт задачи с идентификатором {0}", taskId);

                var importer = new CatalogImporter(taskId);

                importer.Execute();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка импортирования xml каталога продуктов", ex);
                throw;
            }
        }
    }
}
