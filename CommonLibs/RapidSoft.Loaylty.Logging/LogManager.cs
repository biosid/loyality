using System;
using System.Configuration;

namespace RapidSoft.Loaylty.Logging
{
    public static class LogManager
    {
        private static readonly bool UseSerilog = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSerilog"]);

        public static ILog GetLogger(Type type)
        {
            return UseSerilog
                       ? CreateSerilogLogger(type)
                       : CreateLog4NetLogger(type);
        }

        private static ILog CreateSerilogLogger(Type type)
        {
            return new SerilogLog(type);
        }

        private static ILog CreateLog4NetLogger(Type type)
        {
            return new Log(type);
        }
    }
}
