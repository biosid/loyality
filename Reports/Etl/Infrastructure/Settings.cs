using System.Configuration;

namespace Rapidsoft.VTB24.Reports.Etl.Infrastructure
{
    public static class Settings
    {
        public static string EtlConnectionString
        {
            get { return String("vtb24:etl_connection_string", null); }
        }

        public static string EtlSchemaName
        {
            get { return String("vtb24:etl_schema_name", null); }
        }

        public static long MaxReplyDelayHours
        {
            get { return 12; }
        }

        public static string String(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }
    }
}
