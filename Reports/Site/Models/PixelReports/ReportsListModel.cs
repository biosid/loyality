namespace Rapidsoft.VTB24.Reports.Site.Models.PixelReports
{
    public class ReportsListModel
    {
        public const int PAGE_SIZE = 20;

        public ReportBriefModel[] Reports { get; set; }

        public int? NextSkip { get; set; }
    }
}
