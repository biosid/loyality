using System;
using System.ComponentModel;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Monitor.Models
{
    using System.IO;
    using System.Linq;

    public static class EtlPackageMonitorItemStatuses
    {
        public static string ToString(EtlPackageMonitorItemStatus status)
        {
            //todo: localize statuses
            switch (status)
            {
                case EtlPackageMonitorItemStatus.Succeeded:
                    return "Успешно выполнен";
                case EtlPackageMonitorItemStatus.TooFar:
                    return "Выполнен давно";
                case EtlPackageMonitorItemStatus.Running:
                    return "Выполняется...";
                case EtlPackageMonitorItemStatus.Never:
                    return "Ранее не выполнялся";
                case EtlPackageMonitorItemStatus.Failed:
                    return "Выполнен с проблемами";
                default:
                    return "(неизвестно)";
            }
        }

        public static EtlPackageMonitorItemStatus GetMonitorItemStatus(EtlSession session, int runInvervalSeconds, EtlMessage[] messages)
        {
            if (session.EndDateTime.HasValue && (
                    runInvervalSeconds > 0 && DateTime.Now.Subtract(session.EndDateTime.Value).TotalSeconds >= runInvervalSeconds))
            {
                return EtlPackageMonitorItemStatus.TooFar;
            }

            if (session.Status == 0)
            {
                return EtlPackageMonitorItemStatus.Never;
            }

            switch (session.Status)
            {
                case EtlStatus.Waiting:                 
                case EtlStatus.Started:                 
                    return EtlPackageMonitorItemStatus.Running;

                case EtlStatus.FinishedWithWarnings: 
                    return EtlPackageMonitorItemStatus.Warn;
                
                case EtlStatus.FinishedWithLosses: 
                case EtlStatus.FinishedWithErrors:
                case EtlStatus.Failed:                 
                    return EtlPackageMonitorItemStatus.Failed;
                
                default:
                    return GetMonitorItemStatus(messages);                    
            }
        }

        private static EtlPackageMonitorItemStatus GetMonitorItemStatus(EtlMessage[] messages)
        {
            if (messages.Any(m => m.MessageType == EtlMessageType.Warning))
            {
                return EtlPackageMonitorItemStatus.Warn;
            } 
            if (messages.Any(m => m.MessageType == EtlMessageType.Error) ||
                messages.Any(m => m.MessageType == EtlMessageType.Fail))
            {
                return EtlPackageMonitorItemStatus.Failed;
            }

            return EtlPackageMonitorItemStatus.Succeeded;
        }
    }
}
