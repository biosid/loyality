using System.Collections.Generic;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Litres.Reports
{
    public partial class RemainingCodesCountReport
    {
        public IEnumerable<LitresRemainingCodesCount> Codes { get; set; }

        public readonly string ReportRecipients = Configuration.LitresRemainingCodesCountReportTo;
    }
}
