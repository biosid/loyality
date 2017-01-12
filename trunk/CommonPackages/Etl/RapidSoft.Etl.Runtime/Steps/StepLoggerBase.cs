namespace RapidSoft.Etl.Runtime.Steps
{
    using System;

    using RapidSoft.Etl.Logging;

    internal class StepLoggerBase
    {
        private readonly IEtlLogger logger;

        private readonly EtlContext context;

        private readonly string stepName;

        public StepLoggerBase(IEtlLogger logger, EtlContext context, string stepName)
        {
            this.logger = logger;
            this.context = context;
            this.stepName = stepName;
        }

        public void LogError(string message)
        {
            this.Log(EtlMessageType.Error, message);
        }

        public void LogInfo(string message)
        {
            this.Log(EtlMessageType.Information, message);
        }

        public void Log(EtlMessageType type, string message)
        {
            var mess = new EtlMessage
                           {
                               EtlPackageId = this.context.EtlPackageId,
                               EtlSessionId = this.context.EtlSessionId,
                               LogDateTime = DateTime.Now,
                               LogUtcDateTime = DateTime.UtcNow,
                               MessageType = type,
                               Text = message,
                               EtlStepName = this.stepName,
                           };

            this.logger.LogEtlMessage(mess);
        }
    }
}