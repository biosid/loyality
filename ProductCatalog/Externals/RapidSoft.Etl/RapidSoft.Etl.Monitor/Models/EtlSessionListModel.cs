using System.Collections.Generic;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Dumps;

namespace RapidSoft.Etl.Monitor.Models
{
    using RapidSoft.Etl.Runtime;

    public class EtlSessionListModel
    {
        #region Fields

        public readonly int DefaultTimespanDays = 7;
		public readonly int DefaultMaxSessionCount = 250;

        #endregion

        #region Properties

        [DataTypeValidation(DataType.DateTime, ErrorMessage = "Начальная дата указана неверно")]
        public string DateFrom { get; set; }

        [DataTypeValidation(DataType.DateTime, ErrorMessage = "Конечная дата указана неверно")]
        public string DateTo { get; set; }

        public EtlStatus Status { get; set; }

        public List<EtlSession> Sessions { get; set; }

        public string EtlPackagesId { get; set; }

        public IEnumerable<EtlPackage> EtlPackages { get; set; }

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
                    return "";
                case EtlStatus.FinishedWithWarnings:
                    return "yellowbox";
                case EtlStatus.Failed:
                case EtlStatus.FinishedWithLosses:
                default:
                    return "redbox";
            }
        }

        #endregion
    }
}