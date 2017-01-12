using System;

namespace RapidSoft.Loaylty.Logging
{
    public interface ILog
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        void Debug(object message);
        void Debug(object message, Exception exception);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        void DebugFormat(string format, params object[] args);

        void Info(object message);
        void Info(object message, Exception exception);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(string format, params object[] args);

        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(string format, params object[] args);

        void Error(object message);
        void Error(object message, Exception exception);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(string format, params object[] args);

        void Message(LogLevel level, string template, params object[] args);
    }
}
