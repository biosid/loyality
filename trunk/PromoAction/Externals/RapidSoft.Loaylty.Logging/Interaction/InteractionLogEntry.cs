using System;
using System.Collections.Generic;

namespace RapidSoft.Loaylty.Logging.Interaction
{
    internal class InteractionLogEntry : IInteractionLogEntry
    {
        public InteractionStatus Status { get; private set; }

        public Dictionary<string, object> Info { get; private set; }

        public InteractionLogEntry(string subject, string name)
        {
            _subject = subject;
            _name = name;
            _begin = DateTime.Now;
            Status = InteractionStatus.Unknown;
            Info = new Dictionary<string, object>();
        }

        private readonly string _subject;
        private readonly string _name;
        private readonly DateTime _begin;

        private string _failReason;
        private Exception _error;

        public void Succeeded()
        {
            Status = InteractionStatus.Succeeded;
        }

        public void NotSucceeded()
        {
            Status = InteractionStatus.NotSucceeded;
        }

        public void Failed(string reason, Exception error)
        {
            Status = InteractionStatus.Failed;
            _failReason = reason;
            _error = error;
        }

        public void FinishAndWrite(ILog log)
        {
            var end = DateTime.Now;

            var timeTaken = end - _begin;

            var header = string.Format(HEADER_TEMPLATE,
                                       _subject,
                                       _name,
                                       Status,
                                       _begin.ToString("s"),
                                       timeTaken.ToString("c"));

            var details = new
            {
                InteractionSubject = _subject,
                InteractionName = _name,
                BeginTimestamp = _begin,
                EndTimestamp = end,
                TimeTaken = timeTaken,
                Status,
                FailReason = _failReason,
                Error = _error,
                Info
            };

            log.Message(ToLogLevel(Status), LOG_MESSAGE_TEMPLATE, header, details);
        }
    
        private const string HEADER_TEMPLATE = "Interaction with [{0}] for [{1}]: {2}: {3}-{4}";

        private const string LOG_MESSAGE_TEMPLATE = "{Header}::{@Details}";

        private static LogLevel ToLogLevel(InteractionStatus status)
        {
            switch (status)
            {
                case InteractionStatus.Failed:
                    return LogLevel.Error;
                case InteractionStatus.Unknown:
                    return LogLevel.Warning;

                default:
                    return LogLevel.Info;
            }
        }
    }
}
