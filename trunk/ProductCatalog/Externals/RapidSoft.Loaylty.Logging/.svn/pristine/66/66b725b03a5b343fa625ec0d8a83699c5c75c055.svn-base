using System;
using System.Collections.Generic;

namespace RapidSoft.Loaylty.Logging.Interaction
{
    public interface IInteractionLogEntry
    {
        Dictionary<string, object> Info { get; }

        void Succeeded();

        void NotSucceeded();

        void Failed(string reason, Exception error);

        void FinishAndWrite(ILog log);
    }
}
