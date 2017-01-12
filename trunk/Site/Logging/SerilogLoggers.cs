using System;
using System.Configuration;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ElasticSearch;

namespace Vtb24.Logging
{
    public static class SerilogLoggers
    {
        public static ILogger MainLogger
        {
            get
            {
                if (_mainLogger == null)
                {
                    Configure();
                }

                return _mainLogger;
            }
        }

        public static ILogger ServiceLogger
        {
            get
            {
                if (_serviceLogger == null)
                {
                    Configure();
                }

                return _serviceLogger;
            }
        }

        private static ILogger _mainLogger;
        private static ILogger _serviceLogger;

        private static void Configure()
        {
            // для отладки серилога
            //Serilog.Debugging.SelfLog.Out = new StreamWriter(@"c:\temp\serilog.log");

            var logsRoot = ConfigurationManager.AppSettings["logs_root"] ?? @"c:\LoyaltyLogs";
            var applicationName = ConfigurationManager.AppSettings["logs_app_name"] ?? "Front";
            var elasticsearhCluster = ConfigurationManager.AppSettings["logs_elasticsearch_cluster"] ?? "http://localhost:9200";
            var elasticsearchNodes = elasticsearhCluster.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(node => new Uri(node))
                                                        .ToArray();

            _mainLogger = new LoggerConfiguration()
                .WriteTo.RollingFile(logsRoot + @"\" + applicationName + @"\Logs\{Date}.log",
                                     outputTemplate: MAIN_OUTPUT_TEMPLATE,
                                     fileSizeLimitBytes: null,
                                     retainedFileCountLimit: null,
                                     restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.EventLog("Vtb24." + applicationName,
                                  manageEventSource: false,
                                  restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticsearchNodes)
                {
                    MinimumLogEventLevel = LogEventLevel.Information
                })
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.WithProperty("Host", Environment.MachineName)
                .Enrich.WithProperty("Logger", "Main")
                .CreateLogger();

            _serviceLogger = new LoggerConfiguration()
                .WriteTo.RollingFile(logsRoot + @"\" + applicationName + @"\Services\svc.{Date}.log",
                                     outputTemplate: SERVICE_OUTPUT_TEMPLATE,
                                     fileSizeLimitBytes: null,
                                     retainedFileCountLimit: null,
                                     restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticsearchNodes)
                {
                    MinimumLogEventLevel = LogEventLevel.Information
                })
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.WithProperty("Host", Environment.MachineName)
                .Enrich.WithProperty("Logger", "Service")
                .CreateLogger();
        }

        private const string MAIN_OUTPUT_TEMPLATE =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {" + Serilog.Core.Constants.SourceContextPropertyName + "} {Message}{NewLine}{Exception}";

        private const string SERVICE_OUTPUT_TEMPLATE =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {" + Serilog.Core.Constants.SourceContextPropertyName + "} {Message}{NewLine}{Exception}";
    }
}
