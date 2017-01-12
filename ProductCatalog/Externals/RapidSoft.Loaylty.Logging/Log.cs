namespace RapidSoft.Loaylty.Logging
{
	using System;

    /// <summary>
	/// Адаптер логгера log4net.
	/// </summary>
	internal class Log : ILog
	{
        private readonly log4net.ILog _log;
        
		static Log()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

        public bool IsDebugEnabled { get { return _log.IsDebugEnabled; }}
        public bool IsInfoEnabled { get { return _log.IsInfoEnabled; } }
        public bool IsWarnEnabled { get { return _log.IsWarnEnabled; } }

        public Log(Type type)
        {
            _log = log4net.LogManager.GetLogger(type);
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        public void Message(LogLevel level, string template, params object[] args)
        {
            var message = string.Format("MESSAGE:[{0}]",
                                        string.Join("; ", args));

            switch (level)
            {
                case LogLevel.Debug:
                    Debug(message);
                    break;
                case LogLevel.Warning:
                    Warn(message);
                    break;
                case LogLevel.Error:
                    Error(message);
                    break;

                default:
                    Info(message);
                    break;
            }
        }
	}
}
