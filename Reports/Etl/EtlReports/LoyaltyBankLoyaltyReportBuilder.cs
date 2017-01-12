using System;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Helpers;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports
{
    public class LoyaltyBankLoyaltyReportItemsBuilder : IEtlReportItemsBuilder<RequestWithTwoRepliesReportItem>
    {
        public LoyaltyBankLoyaltyReportItemsBuilder(IEtlFiles etlFiles, Guid requestEtlPackageId, Guid repliesEtlPackageId)
        {
            _etlFiles = etlFiles;
            _requestEtlPackageId = requestEtlPackageId;
            _repliesEtlPackageId = repliesEtlPackageId;
        }

        private readonly IEtlFiles _etlFiles;
        private readonly Guid _requestEtlPackageId;
        private readonly Guid _repliesEtlPackageId;

        public RequestWithTwoRepliesReportItem[] Build(DateTime timestamp, DateTime fromDate, DateTime toDate, bool skipRowCountDiscrepancyCheck)
        {
            return _etlFiles
                .GetLoyaltyRequestsWithBankAndLoyaltyReplies(_requestEtlPackageId, _repliesEtlPackageId, fromDate, toDate, ".response", "2")
                .OrderBy(item => item.Item1.Timestamp)
                .Select(item => new RequestWithTwoRepliesReportItem
                {
                    Request = item.Item1,
                    BankReply = item.Item2,
                    LoyaltyReply = item.Item3,
                    IsBankReplyDelayed = item.Item2.IsDelayed(timestamp, item.Item1),
                    IsLoyaltyReplyDelayed = item.Item3.IsDelayed(timestamp, item.Item2),
                    HasBankRowCountDiscrepancy = !skipRowCountDiscrepancyCheck && item.Item1.HasRowCountDiscrepancy(item.Item2),
                    HasLoyaltyRowCountDiscrepancy = !skipRowCountDiscrepancyCheck && item.Item2.HasRowCountDiscrepancy(item.Item3)
                })
                .ToArray();
        }
    }
}
