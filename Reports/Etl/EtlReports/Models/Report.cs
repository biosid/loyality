namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public class Report
    {
        public ReportRequest Request { get; set; }

        public ReportSummary Summary { get; set; }
    }

    public class Report<TReportItem> : Report
    {
        public TReportItem[] Items { get; set; }
    }
}
