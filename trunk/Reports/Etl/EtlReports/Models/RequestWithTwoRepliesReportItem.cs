using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public class RequestWithTwoRepliesReportItem
    {
        public EtlFile Request { get; set; }

        public EtlFile BankReply { get; set; }

        public EtlFile LoyaltyReply { get; set; }

        public bool IsBankReplyDelayed { get; set; }

        public bool IsLoyaltyReplyDelayed { get; set; }

        public bool HasBankRowCountDiscrepancy { get; set; }

        public bool HasLoyaltyRowCountDiscrepancy { get; set; }
    }
}
