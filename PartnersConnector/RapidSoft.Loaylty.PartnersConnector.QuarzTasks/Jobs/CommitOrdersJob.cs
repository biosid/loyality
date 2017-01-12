namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    using System;
    using System.Threading;

    using Quartz;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;

    public class CommitOrdersJob : IInterruptableJob
    {
        public static Func<IQueueProcessor> ProcessorBuilder { get; set; }
        private readonly ILog log = LogManager.GetLogger(typeof (CommitOrdersJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var processorBuilder = ProcessorBuilder;

                if (processorBuilder == null)
                {
                    throw new JobExecutionException("Задача не инициализирована корректно", true, true, true);
                }

                var dataMap = context.JobDetail.JobDataMap;

                var partnerId = dataMap.GetIntValue(DataKeys.PartnerId);

                var processor = processorBuilder();

                processor.Execute(partnerId);
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

        public void Interrupt()
        {
            log.Info("Прерывание выполнения");
            Thread.CurrentThread.Abort();
        }
    }
}