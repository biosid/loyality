using System;
using System.ComponentModel;

namespace RapidSoft.Etl.Monitor.Models
{
    [Serializable]
    public enum EtlPackageMonitorItemStatus
    {
        Succeeded = 1,
        Never = 2,
        Running = 3,
        TooFar = 4,
        Failed = 5,
        Warn = 6
    }
}