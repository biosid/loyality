using System;

namespace Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports
{
    public class ReportBrief
    {
        public Guid Id { get; set; }

        public ReportRequest Request { get; set; }

        public ReportStatus Status { get; set; }

        public DateTime CreateTimestamp { get; set; }

        public int ItemsCount { get; set; }
    }
}
