using System.IO;
using RapidSoft.Loaylty.PartnersConnector.Common;
using Topshelf;

namespace Quartz.Server
{
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs;
    using RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;

    /// <summary>
    /// The server's main entry point.
    /// </summary>
    public static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Main.
        /// </summary>
        public static void Main()
        {
            CommitOrdersJob.ProcessorBuilder = () => new QueueProcessor();
            ImportYmlJob.CatalogAdminServiceProviderBuilder = () => new CatalogAdminServiceProvider();
            ClearDeletedGiftsFilesJob.FileSystemBuilder = () => new FileSystem();

            HostFactory.Run(x =>
                                {
                                    x.RunAsLocalSystem();

                                    x.SetDescription(Configuration.ServiceDescription);
                                    x.SetDisplayName(Configuration.ServiceDisplayName);
                                    x.SetServiceName(Configuration.ServiceName);

                                    x.Service(
                                        factory =>
                                            {
                                                log.Info("Инициализация сервиса Квартц");
                                                QuartzServer server = new QuartzServer();
                                                server.Initialize();
                                                return server;
                                            });
                                });
        }
    }
}