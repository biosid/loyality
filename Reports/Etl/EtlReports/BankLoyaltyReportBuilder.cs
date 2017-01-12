using System;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Helpers;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports
{
    public class BankLoyaltyReportItemsBuilder : IEtlReportItemsBuilder<RequestWithReplyReportItem>
    {
        public BankLoyaltyReportItemsBuilder(IEtlFiles etlFiles, Guid etlPackageId)
        {
            _etlFiles = etlFiles;
            _etlPackageId = etlPackageId;
        }

        private readonly IEtlFiles _etlFiles;
        private readonly Guid _etlPackageId;

        public RequestWithReplyReportItem[] Build(DateTime timestamp, DateTime fromDate, DateTime toDate, bool skipRowCountDiscrepancyCheck)
        {
            var items = _etlFiles.GetBankRequestsWithLoyaltyReplies(_etlPackageId, fromDate, toDate, ".response");

            items = items.GroupBy(item => item.Item1.Name)
                         .Select(group => group.OrderByDescending(item => item.Item1.Timestamp).First())
                         .OrderBy(item => item.Item1.Timestamp)
                         .ToArray();

            return items.Select(item => new RequestWithReplyReportItem
            {
                Request = item.Item1,
                Reply = item.Item2,
                IsReplyDelayed = item.Item2.IsDelayed(timestamp, item.Item1),
                HasRowCountDiscrepancy = !skipRowCountDiscrepancyCheck && item.Item1.HasRowCountDiscrepancy(item.Item2)
            }).ToArray();
        }
    }
}
