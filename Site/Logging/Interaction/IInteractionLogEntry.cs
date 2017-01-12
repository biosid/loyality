using System;
using System.Collections.Generic;
using Serilog;

namespace Vtb24.Logging.Interaction
{
    public interface IInteractionLogEntry
    {
        Dictionary<string, object> Info { get; }

        void Succeeded();

        void NotSucceeded();

        void Failed(string reason, Exception error);

        void FinishAndWrite(ILogger log);
    }
}
