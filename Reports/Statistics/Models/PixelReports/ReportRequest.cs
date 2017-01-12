using System;

namespace Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports
{
    public class ReportRequest
    {
        public string Pixel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
