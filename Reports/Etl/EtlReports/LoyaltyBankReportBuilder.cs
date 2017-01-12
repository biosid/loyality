using System;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Helpers;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports
{
    public class LoyaltyBankReportItemsBuilder : IEtlReportItemsBuilder<RequestWithReplyReportItem>
    {
        public LoyaltyBankReportItemsBuilder(IEtlFiles etlFiles, Guid requestEtlPackageId, Guid replyEtlPackageId)
        {
            _etlFiles = etlFiles;
            _requestEtlPackageId = requestEtlPackageId;
            _replyEtlPackageId = replyEtlPackageId;
        }

        private readonly IEtlFiles _etlFiles;
        private readonly Guid _requestEtlPackageId;
        private readonly Guid _replyEtlPackageId;

        public RequestWithReplyReportItem[] Build(DateTime timestamp, DateTime fromDate, DateTime toDate, bool skipRowCountDiscrepancyCheck)
        {
            return _etlFiles
                .GetLoyaltyRequestsWithBankReplies(_requestEtlPackageId, _replyEtlPackageId, fromDate, toDate, ".response")
                .OrderBy(item => item.Item1.Timestamp)
                .Select(item => new RequestWithReplyReportItem
                {
                    Request = item.Item1,
                    Reply = item.Item2,
                    IsReplyDelayed = item.Item2.IsDelayed(timestamp, item.Item1),
                    HasRowCountDiscrepancy = !skipRowCountDiscrepancyCheck && item.Item1.HasRowCountDiscrepancy(item.Item2)
                })
                .ToArray();
        }
    }
}
