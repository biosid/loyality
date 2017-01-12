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
    public sealed class EtlMailCommand : IEtlConsoleCommand
    {
        #region Constants

        private const string COMMAND_NAME = "mail";
        private const string PARAM_NAME_FROM = "from";
        private const string PARAM_NAME_TO = "to";
        private const string PARAM_NAME_SUBJECT = "subject";
        private const string PARAM_NAME_MAIL_TEMPLATE_PATH = "mailtemplatepath";
        private const string PARAM_NAME_ENABLE_SSL = "enablessl";
        private const string PARAM_NAME_ON_LAST_SECONDS = "onlastseconds";
        private const string PARAM_NAME_ALLOW_EMPTY_MAIL = "allowemptymail";
        private const string PARAM_NAME_ETL_STATUSES = "etlstatuses";
        private const string PARAM_NAME_ETL_PACKAGES = "etlpackages";
        private const string PARAM_NAME_AGENT_TYPE = "agent_type";
        private const string PARAM_NAME_AGENT_CONNECTION_STRING = "agent_connectionstring";
        private const string PARAM_NAME_AGENT_SCHEMA = "agent_schema";

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
                return "sends ETL results by email using command line arguments";
            }
        }

        public string Description
        {
            get
            {
                //todo: localize message
                return "Sends ETL results by email using command line arguments";
            }
        }

        public EtlConsoleCommandOptionInfo[] OptionsInfo
        {
            get
            {
                return new[]
                {
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_FROM, "send email from address"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_TO, "send email to recepients (use ',' as delimiter"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_SUBJECT, "email subject"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_MAIL_TEMPLATE_PATH, "path to email template"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_ENABLE_SSL, "ssl flag", true),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_ON_LAST_SECONDS, "email will contain sessions from this interval"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_ALLOW_EMPTY_MAIL, "allow empty mail without sessions", true),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_ETL_STATUSES, "statuses filter", true),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_TYPE, "type of ETL agent"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_CONNECTION_STRING, "ETL agent connection string"),
                    new EtlConsoleCommandOptionInfo(PARAM_NAME_AGENT_SCHEMA, "ETL agent schema name"),
                };
            }
        }

        #endregion

        #region Methods

        public void ExecuteCommand(EtlConsoleArguments options)
        {
            //todo: localize message
            System.Console.WriteLine("Verifying input parameters...");

            var sb = new StringBuilder();

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_FROM) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_FROM]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_FROM);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_TO) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_TO]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_TO);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_SUBJECT) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_SUBJECT]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_SUBJECT);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_MAIL_TEMPLATE_PATH) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_MAIL_TEMPLATE_PATH]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_MAIL_TEMPLATE_PATH);
            }

            var rc = 0;
            if (!options.CommandOptions.ContainsKey(PARAM_NAME_ON_LAST_SECONDS) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_ON_LAST_SECONDS]) || !Int32.TryParse(options.CommandOptions[PARAM_NAME_ON_LAST_SECONDS], out rc))
            {
                sb.AppendFormat("{0};", PARAM_NAME_ON_LAST_SECONDS);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_TYPE) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_AGENT_TYPE]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_AGENT_TYPE);
            }

            if (!options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_CONNECTION_STRING) || String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_AGENT_CONNECTION_STRING]))
            {
                sb.AppendFormat("{0};", PARAM_NAME_AGENT_CONNECTION_STRING);
            }

            var result = sb.ToString();

            if (String.IsNullOrEmpty(result))
            {
                System.Console.WriteLine("Input parameters are correct");
            }
            else
            {
                throw new Exception(String.Format("Incorrect input parameters: {0}", result));
            }

            Mail(options);
        }

        private static bool Mail(EtlConsoleArguments options)
        {
            System.Console.WriteLine("Creating ETL agent...");
            var agentInfo = new EtlAgentInfo()
            {
                EtlAgentType = options.CommandOptions[PARAM_NAME_AGENT_TYPE],
                ConnectionString = options.CommandOptions[PARAM_NAME_AGENT_CONNECTION_STRING],
                SchemaName = options.CommandOptions.ContainsKey(PARAM_NAME_AGENT_SCHEMA) ? options.CommandOptions[PARAM_NAME_AGENT_SCHEMA] : String.Empty,
            };

            var agent = EtlAgents.CreateAgent(agentInfo);
            System.Console.WriteLine("ETL agent created");

            System.Console.WriteLine("Retrieving dump...");
            List<EtlStatus> statuses = new List<EtlStatus>();
            if (options.CommandOptions.ContainsKey(PARAM_NAME_ETL_STATUSES) && !String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_ETL_STATUSES]))
                foreach (var status in options.CommandOptions[PARAM_NAME_ETL_STATUSES].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    statuses.Add((EtlStatus)Enum.Parse(typeof(EtlStatus), status));

            List<string> etlPackageIds = new List<string>();
            if (options.CommandOptions.ContainsKey(PARAM_NAME_ETL_PACKAGES) && !String.IsNullOrEmpty(options.CommandOptions[PARAM_NAME_ETL_PACKAGES]))
                foreach (var packageId in options.CommandOptions[PARAM_NAME_ETL_PACKAGES].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    etlPackageIds.Add(packageId);

            var dump = GetDump(agent, Convert.ToInt32(options.CommandOptions[PARAM_NAME_ON_LAST_SECONDS]), statuses, etlPackageIds);
            System.Console.WriteLine("Dump has been retrieved");

            System.Console.WriteLine("Sending mail...");
            var allowEmptyMail = true;
            if (options.CommandOptions.ContainsKey(PARAM_NAME_ALLOW_EMPTY_MAIL))
            {
                var rc = false;
                if (Boolean.TryParse(options.CommandOptions[PARAM_NAME_ALLOW_EMPTY_MAIL], out rc))
                    allowEmptyMail = rc;
            }

            var result = false;
            if (dump.Sessions.Count > 0 || (dump.Sessions.Count == 0 && allowEmptyMail))
            {
                var mailBody = GetMailBody(options.CommandOptions[PARAM_NAME_SUBJECT], options.CommandOptions[PARAM_NAME_MAIL_TEMPLATE_PATH], dump);
                SendMail(options, mailBody);
                result = true;
                System.Console.WriteLine("Mail has been sent");
            }
            else
            {
                //todo: localize message
                System.Console.WriteLine("Empty mails is not allowed due to configuration");
                System.Console.WriteLine("Mail has not been sent");
            }

            return result;
        }

        private static void SendMail(EtlConsoleArguments options, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(options.CommandOptions[PARAM_NAME_FROM]),
                Subject = options.CommandOptions[PARAM_NAME_SUBJECT],
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };

            message.To.Add(options.CommandOptions[PARAM_NAME_TO]);

            var enableSsl = false;
            if (options.CommandOptions.ContainsKey(PARAM_NAME_ENABLE_SSL))
            {
                var rc = false;
                if (Boolean.TryParse(options.CommandOptions[PARAM_NAME_ENABLE_SSL], out rc))
                    enableSsl = rc;
            }

            var client = new SmtpClient();
            client.EnableSsl = enableSsl;
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

        private static EtlDump GetDump(IEtlAgent agent, int lastSeconds, List<EtlStatus> etlStatuses, List<string> etlPackageIds)
        {
            var writer = new EtlDumpWriter(new EtlDumpSettings());
            var query = new EtlSessionQuery
            {
                ToDateTime = DateTime.Now,
                FromDateTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(lastSeconds)),
            };
            query.EtlStatuses.AddRange(etlStatuses);
            query.EtlPackageIds.AddRange(etlPackageIds);

            var logParser = agent.GetEtlLogParser();
            writer.Write(query, logParser);

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

        #endregion
    }
}