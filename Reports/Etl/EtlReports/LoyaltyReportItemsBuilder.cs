using System;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports
{
    public class LoyaltyReportItemsBuilder : IEtlReportItemsBuilder<NotificationReportItem>
    {
        public LoyaltyReportItemsBuilder(IEtlFiles etlFiles, Guid etlPackageId)
        {
            _etlFiles = etlFiles;
            _etlPackageId = etlPackageId;
        }

        private readonly IEtlFiles _etlFiles;
        private readonly Guid _etlPackageId;

        public NotificationReportItem[] Build(DateTime timestamp, DateTime fromDate, DateTime toDate, bool skipRowCountDiscrepancyCheck)
        {
            var items = _etlFiles.GetLoyaltyNotificationFiles(_etlPackageId, fromDate, toDate);

            items = items.GroupBy(item => item.Name)
                         .Select(group => group.OrderByDescending(item => item.Timestamp).First())
                         .OrderBy(item => item.Timestamp)
                         .ToArray();

            return items.Select(item => new NotificationReportItem
            {
                Notification = item
            }).ToArray();
        }
    }
}
