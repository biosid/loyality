using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public class RequestWithReplyReportItem
    {
        public EtlFile Request { get; set; }

        public EtlFile Reply { get; set; }

        public bool IsReplyDelayed { get; set; }

        public bool HasRowCountDiscrepancy { get; set; }
    }
}
