using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RapidSoft.Etl.Console
{
    public static class Config
    {
        static readonly bool DEFAULT_EVENT_LOG_ENABLED = false;
        static readonly string DEFAULT_EVENT_LOG_NAME = "EtlConsoleLog";
        static readonly string DEFAULT_EVENT_SOURCE_NAME = "EtlConsoleEventSource";

        public static bool EventLogEnabled
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventLogEnabled"]) ? DEFAULT_EVENT_LOG_ENABLED : Convert.ToBoolean(ConfigurationManager.AppSettings["LogErrors"]);
            }
        }

        public static string EventLogName 
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventLogName"]) ? DEFAULT_EVENT_LOG_NAME : ConfigurationManager.AppSettings["EventLogName"];
            }
        }

        public static string EventSourceName
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventSourceName"]) ? DEFAULT_EVENT_SOURCE_NAME : ConfigurationManager.AppSettings["EventSourceName"];
            }
        }
    }
}
