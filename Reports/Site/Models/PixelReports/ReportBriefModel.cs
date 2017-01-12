using System;

namespace Rapidsoft.VTB24.Reports.Site.Models.PixelReports
{
    public class ReportBriefModel
    {
        public Guid? Id { get; set; }

        public string Status { get; set; }

        public DateTime CreateTimestamp { get; set; }

        public string Pixel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int? ItemsCount { get; set; }
    }
}
