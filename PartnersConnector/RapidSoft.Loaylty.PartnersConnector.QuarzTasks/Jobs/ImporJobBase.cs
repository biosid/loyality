namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    using Quartz;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;

    public abstract class ImporJobBase : IInterruptableJob
    {
        public static Func<ICatalogAdminServiceProvider> CatalogAdminServiceProviderBuilder { get; set; }

        protected void LoadFile(string source, string destination)
        {
            try
            {
                Logger.DebugFormat("Загрузка файла {0} и сохранение как {1}", source, destination);

                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                var webRequest = WebRequest.Create(source);
                webRequest.Method = "GET";

                using (var response = webRequest.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var fileStream = File.Create(destination))
                        {
                            var buffer = new byte[1024];
                            int bytesRead;
                            do
                            {
                                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                            while (bytesRead > 0);
                        }
                    }
                }

                Logger.DebugFormat("Файл загружен");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка загрузки файла", ex);
                throw;
            }
        }

        public abstract void Execute(IJobExecutionContext context);

        protected ICatalogAdminServiceProvider GetCatalogAdminServiceProvider()
        {
            var catalogAdminServiceProviderBuilder = CatalogAdminServiceProviderBuilder;

            if (catalogAdminServiceProviderBuilder == null)
            {
                throw new JobExecutionException("Задача не инициализирована корректно", true, true, true);
            }

            var provider = catalogAdminServiceProviderBuilder();
            return provider;
        }

        public virtual void Interrupt()
        {
            Logger.Info("Прерывание выполнения");
            Thread.CurrentThread.Abort();
        }
    }
}