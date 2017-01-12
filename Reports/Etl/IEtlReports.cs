using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl
{
    public interface IEtlReports
    {
        Report<NotificationReportItem> LoyaltyReport(ReportRequest request);

        Report<RequestWithReplyReportItem> BankLoyaltyReport(ReportRequest request);

        Report<RequestWithReplyReportItem> LoyaltyBankReport(ReportRequest request);

        Report<RequestWithTwoRepliesReportItem> LoyaltyBankLoyaltyReport(ReportRequest request);
    }
}
