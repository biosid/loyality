namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Logging.Sql;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public static class WrapperExtensions
    {
        internal static bool IsSuccess(this WrapperBase job)
        {
            var etlLogger = new SqlEtlLogParser(ConfigHelper.ConnectionString, "etl");
            var messages = etlLogger.GetEtlMessages(job.PackageId, job.SessionId);
            var endOfSteps = messages.Where(t => t.MessageType == EtlMessageType.StepEnd || t.MessageType == EtlMessageType.SessionEnd);
            return endOfSteps.All(t => t.Text.Contains("Succeeded") || t.Text.Contains("FinishedWithSessionEnd"));
        }

        internal static bool IsError(this WrapperBase job)
        {
            var etlLogger = new SqlEtlLogParser(ConfigHelper.ConnectionString, "etl");
            var messages = etlLogger.GetEtlMessages(job.PackageId, job.SessionId);
            var endOfSteps = messages.Where(t => t.MessageType == EtlMessageType.StepEnd || t.MessageType == EtlMessageType.SessionEnd);
            return endOfSteps.All(t => t.Text.Contains("FinishedWithErrors"));
        }

        internal static string GetMessagesText(this WrapperBase job, EtlMessage[] messages = null)
        {
            if (messages == null)
            {
                messages = GetMessages(job, EtlMessageType.Error);    
            }

            return string.Join(Environment.NewLine, messages.Select(m => string.Format("Text:{0} Stack:{1}", m.Text, m.StackTrace)));
        }

        internal static EtlMessage[] GetMessages(this WrapperBase job, EtlMessageType etlMessageType)
        {
            var etlLogger = new SqlEtlLogParser(ConfigHelper.ConnectionString, "etl");
            var messages = etlLogger.GetEtlMessages(job.PackageId, job.SessionId);
            var errorMessages = messages.Where(t => t.MessageType == etlMessageType).ToArray();
            return errorMessages;
        }

        internal static bool IsFinishedWithLosses(this WrapperBase job)
        {
            var etlLogger = new SqlEtlLogParser(ConfigHelper.ConnectionString, "etl");
            var messages = etlLogger.GetEtlMessages(job.PackageId, job.SessionId);
            return messages.Any(t => t.MessageType == EtlMessageType.SessionEnd && t.Text.Contains("FinishedWithLosses"));
        }
    }
}