namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.IO;

    using Ionic.Zip;

    using RapidSoft.Etl.Logging;

    [Serializable]
    public sealed class EtlCompressStep : EtlStep
    {
        private const string COMPRESS_EXTENSION = ".zip";

        [Category("1. WorkingDirectory")]
        public string WorkingDirectory { get; set; }

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (string.IsNullOrWhiteSpace(WorkingDirectory))
            {
                throw new InvalidOperationException("WorkingDirectory cannot be empty");
            }

            try
            {
                logger.LogEtlMessage(
                    new EtlMessage
                    {
                        EtlPackageId = context.EtlPackageId,
                        EtlSessionId = context.EtlSessionId,
                        EtlStepName = Name,
                        LogDateTime = DateTime.Now,
                        LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                        MessageType = EtlMessageType.Information,
                        Text = string.Format("Start compression for directory({0})", WorkingDirectory),
                    });
                CompressFiles(context, logger);
                logger.LogEtlMessage(
                    new EtlMessage
                    {
                        EtlPackageId = context.EtlPackageId,
                        EtlSessionId = context.EtlSessionId,
                        EtlStepName = Name,
                        LogDateTime = DateTime.Now,
                        LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                        MessageType = EtlMessageType.Information,
                        Text = string.Format("Compression completed for directory({0})", WorkingDirectory),
                    });
            }
            catch (Exception error)
            {
                logger.LogEtlMessage(
                    new EtlMessage
                    {
                        EtlPackageId = context.EtlPackageId,
                        EtlSessionId = context.EtlSessionId,
                        EtlStepName = Name,
                        LogDateTime = DateTime.Now,
                        LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                        MessageType = EtlMessageType.Error,
                        Text = error.Message,
                        StackTrace = error.StackTrace
                    });
                return new EtlStepResult(EtlStatus.Failed, error.Message);
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        private void CompressFiles(EtlContext context, IEtlLogger logger)
        {
            var workingDirectoryInfo = new DirectoryInfo(WorkingDirectory);
            foreach (var file in workingDirectoryInfo.GetFiles())
            {
                Compress(file);
                ReplaceFile(file);

                file.Refresh();
                LogFileSize(context, logger, file);
            }
        }

        private static void Compress(FileSystemInfo fileToCompress)
        {
            if (fileToCompress.Extension == COMPRESS_EXTENSION)
            {
                return;
            }

            using (var zf = new ZipFile(fileToCompress.FullName + COMPRESS_EXTENSION))
            {
                zf.AddFile(fileToCompress.FullName, string.Empty);
                zf.Save();
            }
        }

        private static void ReplaceFile(FileSystemInfo file)
        {
            var filePath = file.FullName;
            file.Delete();
            File.Move(filePath + COMPRESS_EXTENSION, filePath);
        }

        private static void LogFileSize(EtlContext context, IEtlLogger logger, FileInfo file)
        {
            var now = DateTime.Now;

            logger.LogEtlCounter(new EtlCounter
            {
                EtlPackageId = context.EtlPackageId,
                EtlSessionId = context.EtlSessionId,
                EntityName = Path.GetFileName(file.Name),
                CounterName = "CompressedSize",
                CounterValue = file.Length,
                DateTime = now,
                UtcDateTime = now.ToUniversalTime()
            });
        }
    }
}