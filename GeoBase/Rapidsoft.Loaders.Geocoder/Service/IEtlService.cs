using System;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Loaders.Geocoder.Service
{
    public interface IEtlService
    {
        Guid EtlPackageId { get; }

        Guid EtlSessionId { get; }

        IEtlLogger EtlLogger { get; }

        void AddMessage(string message);

        void AddMessageWithException(Exception ex);

        void StartSession(string message);

        void EndSession(string message, EtlStatus status);
    }
}
