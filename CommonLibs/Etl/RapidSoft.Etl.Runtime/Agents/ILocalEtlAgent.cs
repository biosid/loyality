using System;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Agents
{
    public interface ILocalEtlAgent : IEtlAgent
    {
        void AttachLogger(IEtlLogger logger);
    }
}