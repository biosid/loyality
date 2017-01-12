using System;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public class ReportRequest
    {
        public InteractionType Type { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool SkipRowCountDiscrepancyCheck { get; set; }
    }
}
