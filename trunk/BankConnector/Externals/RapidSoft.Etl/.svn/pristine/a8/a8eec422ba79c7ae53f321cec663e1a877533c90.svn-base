using System;
using System.Collections.Generic;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Monitor.Models
{
    public class EtlPackageMonitorItem
    {
        #region Properties

        public string EtlPackageId
        {
            get;
            set;
        }

        public string EtlSessionId
        {
            get;
            set;
        }

        public string EtlPackageName
        {
            get;
            set;
        }

        public EtlPackageMonitorItemStatus EtlPackageStatus
        {
            get;
            set;
        }

        public string StatusMessage
        {
            get;
            set;
        }

        public DateTime? EtlSessionDateTime
        {
            get;
            set;
        }

        public int RunIntervalSeconds
        {
            get;
            set;
        }

        public bool CanInvoke
        {
            get;
            set;
        }

        public bool ForceRefresh
        {
            get;
            set;
        }

        public EtlCounter[] Counters
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static string GetEtlStatusClass(EtlStatus status)
        {
            switch (status)
            {
                case EtlStatus.Waiting:
                    return "graybox";
                case EtlStatus.Started:
                    return "yellowbox";
                case EtlStatus.Succeeded:
                    return "greenbox";
                case EtlStatus.Failed:
                case EtlStatus.FinishedWithLosses:
                case EtlStatus.FinishedWithWarnings:
                default:
                    return "redbox";
            }
        }

        public void UpdateFromItem(EtlPackageMonitorItem item)
        {
            EtlPackageId = item.EtlPackageId;
            EtlSessionId = item.EtlSessionId;
            EtlPackageName = item.EtlPackageName;
            EtlPackageStatus = item.EtlPackageStatus;
            StatusMessage = item.StatusMessage;
            //EtlSessionStatus = item.EtlSessionStatus;
            EtlSessionDateTime = item.EtlSessionDateTime;
            CanInvoke = item.CanInvoke;
            ForceRefresh = item.ForceRefresh;
        }

        #endregion
    }
}