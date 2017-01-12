using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Dumps;

namespace RapidSoft.Etl.Console
{
    public sealed class EtlMailfCommand : IEtlConsoleCommand
    {
        #region Constants

        private const string COMMAND_NAME = "mailf";
        private const string CONFIGFILEPATH_PARAM_NAME = "configfilepath";

        #endregion

        #region Properties

        public string CommandName
        {
            get
            {
                return COMMAND_NAME;
            }
        }

        public string ShortDescription
        {
            get
            {
                //todo: localize message
                return "sends ETL results by email using settings from <configfilepath>";
            }
        }

        public string Description
        {
            get
            {
                //todo: localize message
                return "Sends ETL results by email using settings from <configfilepath>";
            }
        }

        public EtlConsoleCommandOptionInfo[] OptionsInfo
        {
            get
            {
                return new []
                {
                    new EtlConsoleCommandOptionInfo(CONFIGFILEPATH_PARAM_NAME, "path to mail configuration file")
                };
            }
        }

        #endregion

        #region Methods

        public void ExecuteCommand(EtlConsoleArguments options)
        {
            //todo: localize message
            System.Console.WriteLine("Reading mail configuration...");

            //todo: handle exception KeyNotFound
            var configFilePath = options.CommandOptions[CONFIGFILEPATH_PARAM_NAME];
            var config = ReadConfiguration(configFilePath);

            //todo: localize message
            System.Console.WriteLine("Mail configuration has been read");

            Mail(config);
        }

        private EtlMailReportConfiguration ReadConfiguration(string configFilePath)
        {
            var ser = new XmlSerializer(typeof(EtlMailReportConfiguration));
            using (var reader = new XmlTextReader(configFilePath))
            {
                return (EtlMailReportConfiguration)ser.Deserialize(reader);
            }
        }

        private void Mail(EtlMailReportConfiguration config)
        {
            foreach (var subscription in config.Subscriptions)
            {
                //todo: localize message
                System.Console.WriteLine("Sending mail \"{0}\"...", subscription.Subject);
                
                var result = Mail(subscription);
                
                //todo: localize message
                System.Console.WriteLine(result ? "Mail has been sent" : "Mail has not been sent");
            }
        }

        private static bool Mail(EtlMailSubscription subscription)
        {
            System.Console.WriteLine("Retrieving dump for \"{0}\"...", subscription.Subject);
            var dump = GetDump(subscription);
            System.Console.WriteLine("Dump for \"{0}\" retrieved", subscription.Subject);

            var result = false;
            if (dump.Sessions.Count > 0 || (dump.Sessions.Count == 0 && subscription.AllowEmptyMail))
            {
                var mailBody = GetMailBody(subscription.Subject, subscription.MailTemplatePath, dump);
                SendMail(subscription, mailBody);
                result = true;
            }
            else
            {
                //todo: localize message
                System.Console.WriteLine("Empty mail is not allowed due to configuration");
            }

            return result;
        }

        private static void SendMail(EtlMailSubscription subscription, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(subscription.From),
                Subject = subscription.Subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };

            foreach (var subscriberInfo in subscription.Subscribers)
            {
                message.To.Add(subscriberInfo.Email);
            }

            var client = new SmtpClient();
            client.EnableSsl = subscription.EnableSsl;
            client.Send(message);
        }

        private static string GetMailBody(string subject, string templatePath, EtlDump dump)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var serializer = new XmlSerializer(typeof(EtlDump));
                serializer.Serialize(writer, dump);
            }

            var trans = new XslCompiledTransform();
            using (var xsltStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var xmlReader = XmlReader.Create(xsltStream);
                trans.Load(xmlReader);
            }

            using (var sr = new StringReader(sb.ToString()))
            {
                var xmlReader = XmlReader.Create(sr);
                var writer = new StringWriter();
                var xmlWriter = XmlWriter.Create(writer, trans.OutputSettings);
                var xsltArgumentList = new XsltArgumentList();
                xsltArgumentList.AddParam("subject", "", subject);
                trans.Transform(xmlReader, xsltArgumentList, xmlWriter);

                return writer.ToString();
            }
        }

        private static EtlDump GetDump(EtlMailSubscription subscription)
        {
            var writer = new EtlDumpWriter(new EtlDumpSettings());

            foreach (var agentInfo in subscription.EtlAgents)
            {
                var query = CreateSessionQuery(subscription);
                var agent = EtlAgents.CreateAgent(agentInfo);
                var logParser = agent.GetEtlLogParser();
                writer.Write(query, logParser);
            }

            var dump = writer.GetDump();
            dump.Sessions.Sort(new Comparison<EtlSessionSummary>(SortSessionsDesc));
            return dump;
        }

        private static int SortSessionsDesc(EtlSessionSummary x, EtlSessionSummary y)
        {
            if (x.StartDateTime == y.StartDateTime)
            {
                return 0;
            }
            else if (x.StartDateTime > y.StartDateTime)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private static EtlSessionQuery CreateSessionQuery(EtlMailSubscription subscription)
        {
            var query = new EtlSessionQuery
            {
                ToDateTime = DateTime.Now,
                FromDateTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(subscription.OnLastSeconds)),
            };

            query.EtlStatuses.AddRange(subscription.EtlStatuses);

            return query;
        }

        #endregion
    }
}