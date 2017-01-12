namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks.Jobs
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    using Quartz;

    using Logging;

    public abstract class ImportJobBase : JobBase, IInterruptableJob
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ImportJobBase));

        protected void LoadFile(string source, string destination)
        {
            try
            {
                log.InfoFormat("Загрузка файла {0} и сохранение как {1}", source, destination);

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

                log.InfoFormat("Файл загружен");
            }
            catch (Exception ex)
            {
                log.Error("Ошибка загрузки файла", ex);
                throw;
            }
        }

        public abstract void Execute(IJobExecutionContext context);

        public virtual void Interrupt()
        {
            log.Info("Прерывание выполнения");
            Thread.CurrentThread.Abort();
        }
    }
}