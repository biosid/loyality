using System;

namespace Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports
{
    public class ReportItem
    {
        public DateTime Timestamp { get; set; }

        public string IpAddress { get; set; }

        public string Agent { get; set; }
    }
}
