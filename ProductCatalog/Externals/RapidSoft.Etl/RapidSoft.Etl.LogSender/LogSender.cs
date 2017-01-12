namespace RapidSoft.Etl.LogSender
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Xsl;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Logging.Dumps;
    using RapidSoft.Etl.Runtime.Agents;

    /// <summary>
    /// Отправщик логов сессии по электронной почты. 
    /// NOTE: Настройки подключения в SMTP-серверу в config-файле в секции system.net/mailSettings/smtp!
    /// </summary>
    public static class LogSender
    {
        public static void SendSessionLog(this IEtlAgent agent, EtlSession etlSession, string subject, string recipient)
        {
            var recipients = new[]
                             {
                                 recipient
                             };

            agent.SendSessionLog(etlSession, subject, recipients);
        }

        public static void SendSessionLog(this IEtlAgent agent, EtlSession etlSession, string subject, string[] recipients, ILogEmailSender logEmailSender = null, int? errorTakeCount = 100)
        {
            var dump = agent.GetSessionDump(etlSession, errorTakeCount);

            var mailBody = dump.FormatMail(subject, "RapidSoft.Etl.LogSender.MailTemplates.ReportBySession.xslt");

            if (logEmailSender == null)
            {
                throw new Exception("IEmailSender is null");
            }

            logEmailSender.SendMail(subject, recipients, mailBody);
        }

        public static string FormatMail(this EtlDump dump, string subject, string templatePath)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(EtlDump));
                serializer.Serialize(writer, dump);
            }

            var trans = new XslCompiledTransform();

            var ass = typeof(LogSender).Assembly;
            using (Stream stream = ass.GetManifestResourceStream(templatePath))
            {
                var xmlReader = XmlReader.Create(stream);
                trans.Load(xmlReader);
            }

            using (var sr = new StringReader(sb.ToString()))
            {
                var xmlReader = XmlReader.Create(sr);
                var writer = new StringWriter();
                var xmlWriter = XmlWriter.Create(writer, trans.OutputSettings);
                var xsltArgumentList = new XsltArgumentList();
                xsltArgumentList.AddParam("subject", string.Empty, subject);
                trans.Transform(xmlReader, xsltArgumentList, xmlWriter);

                return writer.ToString();
            }
        }

        public static EtlDump GetSessionDump(this IEtlAgent agent, string etlPackageId, string etlSessionId, int? errorTakeCount = 100)
        {
            List<EtlStatus> etlStatuses = new List<EtlStatus>
                                              {
                                                  EtlStatus.Waiting,
                                                  EtlStatus.Failed,
                                                  EtlStatus.FinishedWithLosses,
                                                  EtlStatus.FinishedWithWarnings,
                                                  EtlStatus.Started,
                                                  EtlStatus.Succeeded
                                              };

            var etlDumpSettings = new EtlDumpSettings(int.MaxValue, 0);
            var writer = new EtlDumpWriter(etlDumpSettings);
            var query = new EtlSessionQuery
            {
                ToDateTime = DateTime.Now,
                FromDateTime = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
            };
            query.EtlStatuses.AddRange(etlStatuses);
            query.EtlPackageIds.Add(etlPackageId);

            var logParser = agent.GetEtlLogParser();
            writer.Write(query, logParser);

            var dump = writer.GetDump();
            var sessionSummary =
                dump.Sessions.Where(x => x.EtlSessionId == etlSessionId).Select(x => TakeMessages(x, errorTakeCount)).ToList();

            dump.Sessions.Clear();

            dump.Sessions.AddRange(sessionSummary);

            return dump;
        }

        public static EtlDump GetSessionDump(this IEtlAgent agent, EtlSession etlSession, int? errorTakeCount = 100)
        {
            return agent.GetSessionDump(etlSession.EtlPackageId, etlSession.EtlSessionId, errorTakeCount);
        }

        private static EtlSessionSummary TakeMessages(EtlSessionSummary sessionSummary, int? errorTakeCount = 100)
        {
            if (!errorTakeCount.HasValue)
            {
                return sessionSummary;
            }

            var messages = new List<EtlMessage>();
            var count = 0;

            foreach (var message in sessionSummary.Messages)
            {
                if (message.MessageType == EtlMessageType.Error)
                {
                    if (count < errorTakeCount.Value)
                    {
                        messages.Add(message);
                        count++;
                    }
                }
                else
                {
                    messages.Add(message);
                }
            }

            sessionSummary.Messages = messages;
            return sessionSummary;
        }
    }
}