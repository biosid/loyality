using System;

namespace RapidSoft.Loaylty.Logging
{
    public static class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new Log(type);
        }
    }
}
