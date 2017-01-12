using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ElasticSearch;

namespace RapidSoft.Loaylty.Logging
{
    internal class SerilogLog : ILog
    {
        private const string OUTPUT_TEMPLATE =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {" + Serilog.Core.Constants.SourceContextPropertyName + "} {Message}{NewLine}{Exception}";

        private const string MESSAGE_TEMPLATE = "{$LogMessage}";

        private static readonly ILogger Logger;

        static SerilogLog()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SerilogDebugEnabled"]))
            {
                Serilog.Debugging.SelfLog.Out = new StreamWriter(@"c:\temp\serilog.log");
            }

            var logsRoot = ConfigurationManager.AppSettings["LogFilesRoot"] ?? @"c:\LoyaltyLogs";
            var applicationName = ConfigurationManager.AppSettings["LogAppName"] ?? "Back";
            var subSystemName = ConfigurationManager.AppSettings["LogSubsystemName"] ?? "Logs";
            var eventLogSourceName = ConfigurationManager.AppSettings["EventLogSourceName"];
            var elasticSearchNodes = ConfigurationManager.AppSettings["LogsElasticSearchNodes"] ?? string.Empty;

            var elasticSearchUris = elasticSearchNodes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(node => new Uri(node))
                                                      .ToArray();

            var config = new LoggerConfiguration()
                .WriteTo.RollingFile(logsRoot + @"\" + applicationName + @"\" + subSystemName + @"\{Date}.log",
                                     outputTemplate: OUTPUT_TEMPLATE,
                                     fileSizeLimitBytes: null,
                                     retainedFileCountLimit: null,
                                     restrictedToMinimumLevel: LogEventLevel.Information);

            if (!string.IsNullOrWhiteSpace(eventLogSourceName))
            {
                config = config.WriteTo.EventLog(eventLogSourceName,
                                                 logName: "VTB24.Loaylty",
                                                 manageEventSource: false,
                                                 restrictedToMinimumLevel: LogEventLevel.Warning);
            }

            if (elasticSearchUris.Length > 0)
            {
                config = config.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticSearchUris)
                {
                    MinimumLogEventLevel = LogEventLevel.Information
                });
            }

            Logger = config.Enrich.WithProperty("Application", applicationName + "." + subSystemName)
                           .Enrich.WithProperty("Host", Environment.MachineName)
                           .CreateLogger();
        }

        public SerilogLog(Type type)
        {
            _log = Logger.ForContext(type);
        }

        private readonly ILogger _log;

        public bool IsDebugEnabled { get { return _log.IsEnabled(LogEventLevel.Debug); } }

        public bool IsInfoEnabled { get { return _log.IsEnabled(LogEventLevel.Information); } }

        public bool IsWarnEnabled { get { return _log.IsEnabled(LogEventLevel.Warning); } }

        public void Debug(object message)
        {
            _log.Debug(MESSAGE_TEMPLATE, message);
        }

        public void Debug(object message, Exception exception)
        {
            _log.Debug(exception, MESSAGE_TEMPLATE, message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.Debug(MESSAGE_TEMPLATE, string.Format(provider, format, args));
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.Debug(MESSAGE_TEMPLATE, string.Format(format, args));
        }

        public void Info(object message)
        {
            _log.Information(MESSAGE_TEMPLATE, message);
        }

        public void Info(object message, Exception exception)
        {
            _log.Information(exception, MESSAGE_TEMPLATE, message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.Information(MESSAGE_TEMPLATE, string.Format(provider, format, args));
        }

        public void InfoFormat(string format, params object[] args)
        {
            _log.Information(MESSAGE_TEMPLATE, string.Format(format, args));
        }

        public void Warn(object message)
        {
            _log.Warning(MESSAGE_TEMPLATE, message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warning(exception, MESSAGE_TEMPLATE, message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.Warning(MESSAGE_TEMPLATE, string.Format(provider, format, args));
        }

        public void WarnFormat(string format, params object[] args)
        {
            _log.Warning(MESSAGE_TEMPLATE, string.Format(format, args));
        }

        public void Error(object message)
        {
            _log.Error(MESSAGE_TEMPLATE, message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(exception, MESSAGE_TEMPLATE, message);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.Error(MESSAGE_TEMPLATE, string.Format(provider, format, args));
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.Error(MESSAGE_TEMPLATE, string.Format(format, args));
        }

        public void Message(LogLevel level, string template, params object[] args)
        {
            _log.Write(SerilogMappings.ToLogEventLevel(level), template, args);
        }
    }
}
