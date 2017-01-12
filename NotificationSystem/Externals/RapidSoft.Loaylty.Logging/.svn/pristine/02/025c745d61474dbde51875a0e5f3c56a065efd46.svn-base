using Serilog.Events;

namespace RapidSoft.Loaylty.Logging
{
    internal static class SerilogMappings
    {
        public static LogEventLevel ToLogEventLevel(LogLevel original)
        {
            switch (original)
            {
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Info:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;

                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
