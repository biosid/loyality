using System;
using System.Collections.Generic;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Dumps;

namespace RapidSoft.Etl.Monitor.Models
{
    public class EtlSessionViewModel
	{
        #region Properties

        public string BackUrl { get; set; }

        public bool NotFound { get; set; }

        public EtlSessionSummary Session { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool PageFromRequest { get; set; }

        #endregion

        #region Methods

        public static string GetEtlStatusClass(EtlStatus status)
        {
            switch (status)
            {
                case EtlStatus.Waiting:
                    return "graybox";
                case EtlStatus.Started:
                    return "bluebox";
                case EtlStatus.Succeeded:
                    //return "greenbox";
                    return "";
                case EtlStatus.FinishedWithWarnings:
                    return "yellowbox";
                case EtlStatus.Failed:
                case EtlStatus.FinishedWithLosses:
                default:
                    return "redbox";
            }
        }

        public static string GetEtlMessageClass(EtlMessageType type)
        {
            switch (type)
            {
                case EtlMessageType.StepStart:
                case EtlMessageType.StepEnd:
                case EtlMessageType.SessionStart:
                case EtlMessageType.SessionEnd:
                    return "bluebox";
                case EtlMessageType.Warning:
                    return "yellowbox";
                case EtlMessageType.Error:
                    return "redbox";
                default:
                    return "";
            }
        }

        #endregion
	}
}