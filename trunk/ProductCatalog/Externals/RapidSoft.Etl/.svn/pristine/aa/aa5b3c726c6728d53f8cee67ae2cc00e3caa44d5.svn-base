namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;

    using RapidSoft.Etl.Logging;
    using RapidSoft.VTB24.VtbEncryption;

    [Serializable]
    public sealed class EtlEncryptStep : EtlStep
    {
        [Category("1. WorkingDirectory")]
        public string WorkingDirectory
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

            if (this.WorkingDirectory != null)
            {
                if (string.IsNullOrEmpty(this.WorkingDirectory))
                {
                    throw new InvalidOperationException("WorkingDirectory cannot be empty");
                }
            }

            try
            {
				logger.LogEtlMessage(
					new EtlMessage
					{
						EtlPackageId = context.EtlPackageId,
						EtlSessionId = context.EtlSessionId,
						EtlStepName = this.Name,
						LogDateTime = DateTime.Now,
						LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
						MessageType = EtlMessageType.Information,
						Text = string.Format("Start encrypt for directory({0})", this.WorkingDirectory),
					});
				VtbEncryptionHelper.Encrypt(this.WorkingDirectory);
				logger.LogEtlMessage(
					new EtlMessage
					{
						EtlPackageId = context.EtlPackageId,
						EtlSessionId = context.EtlSessionId,
						EtlStepName = this.Name,
						LogDateTime = DateTime.Now,
						LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
						MessageType = EtlMessageType.Information,
						Text = string.Format("Encrypt completed for directory({0})", this.WorkingDirectory),
					});
			}
            catch (Exception ex)
            {
                logger.LogEtlMessage(
                    new EtlMessage
                        {
                            EtlPackageId = context.EtlPackageId,
                            EtlSessionId = context.EtlSessionId,
                            EtlStepName = this.Name,
                            LogDateTime = DateTime.Now,
                            LogUtcDateTime = DateTime.UtcNow.ToUniversalTime(),
                            MessageType = EtlMessageType.Error,
                            Text = ex.Message,
                            StackTrace = ex.StackTrace
                        });
                return new EtlStepResult(EtlStatus.Failed, ex.Message);
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }
    }
}