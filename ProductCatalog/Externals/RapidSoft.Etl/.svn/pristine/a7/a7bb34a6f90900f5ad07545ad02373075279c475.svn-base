using System;

namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using RapidSoft.Etl.Logging;

    [Serializable]
    public sealed class EtlExecStep : EtlStep
    {
        [Category("1. Run")]
        public EtlFileInfo Run
        {
            get;
            set;
        }

        [Category("1. Run")]
        public string Arguments
        {
            get;
            set;
        }

        [Category("2. FilePro Specific")]
        public EtlFileInfo Source
        {
            get;
            set;
        }

        [Category("2. FilePro Specific")]
        public EtlFileInfo Destination
        {
            get;
            set;
        }

        [Category("2. FilePro Specific")]
        public EtlFileInfo KeysStore
        {
            get;
            set;
        }

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

            if (this.Source != null)
            {
                if (string.IsNullOrEmpty(this.Source.FilePath))
                {
                    throw new InvalidOperationException("Source.FilePath cannot be empty");
                }
            }

            if (this.Destination != null)
            {
                if (string.IsNullOrEmpty(this.Destination.FilePath))
                {
                    throw new InvalidOperationException("Destination.FilePath cannot be empty");
                }
            }

            if (this.KeysStore != null)
            {
                if (string.IsNullOrEmpty(this.KeysStore.FilePath))
                {
                    throw new InvalidOperationException("KeysStore.FilePath cannot be empty");
                }
            }

            var args = String.Format(" {0}", this.Arguments);
            if (this.Source != null && this.Destination != null && this.KeysStore != null)
            {
                args = String.Format(" {0} {1} {2} {3}", this.Arguments, this.Source.FilePath, this.Destination.FilePath, this.KeysStore.FilePath);
            }

            logger.LogEtlMessage(new EtlMessage
            {
                EtlPackageId = context.EtlPackageId,
                EtlSessionId = context.EtlSessionId,
                EtlStepName = this.Name,
                LogDateTime = DateTime.Now,
                LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                MessageType = EtlMessageType.Information,
                Text = args,
            });

            try
            {
                using (var exeProcess = new Process())
                {
                    exeProcess.StartInfo.CreateNoWindow = true;
                    exeProcess.StartInfo.UseShellExecute = false;
                    exeProcess.StartInfo.FileName = this.Run.FilePath;
                    exeProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    exeProcess.StartInfo.Arguments = args;
                    
                    exeProcess.Start();

                    exeProcess.WaitForExit();

                    var resultCode = exeProcess.ExitCode;

                    if (resultCode != 0)
                    {
                        var mess = string.Format(
                            "Выполнение {0} с аргументами {1} завершилось с кодом{2}",
                            this.Run.FilePath,
                            args,
                            resultCode);

                        throw new Exception(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogEtlMessage(new EtlMessage
                {
                    EtlPackageId = context.EtlPackageId,
                    EtlSessionId = context.EtlSessionId,
                    EtlStepName = this.Name,
                    LogDateTime = DateTime.Now,
                    LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                    MessageType = EtlMessageType.Error,
                    Text = args,
                    StackTrace = ex.StackTrace
                });
                return new EtlStepResult(EtlStatus.Failed, ex.Message);
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }
    }
}