using System;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public class ReportSummary
    {
        public DateTime Timestamp { get; set; }

        public bool ShowReplyDelayed { get; set; }

        public bool ShowRowCountDiscrepancy { get; set; }

        public bool ShowSizeExceeded { get; set; }

        public bool HasAnyReplyDelayed { get; set; }

        public bool HasAnyRowCountDiscrepancy { get; set; }

        public bool HasAnySizeExceeded { get; set; }
    }
}
