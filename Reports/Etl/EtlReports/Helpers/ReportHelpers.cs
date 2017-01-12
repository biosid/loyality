using System;
using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;
using Rapidsoft.VTB24.Reports.Etl.Infrastructure;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Helpers
{
    internal static class ReportHelpers
    {
        public static bool IsDelayed(this EtlFile reply, DateTime timestamp, EtlFile request)
        {
            return
                request != null &&
                reply == null &&
                (timestamp - request.Timestamp).TotalHours >= Settings.MaxReplyDelayHours;
        }

        public static bool HasRowCountDiscrepancy(this EtlFile request, EtlFile reply)
        {

            return
                request != null &&
                reply != null &&
                request.RowCount.HasValue &&
                reply.RowCount.HasValue &&
                reply.RowCount.Value != request.RowCount.Value;
        }
    }
}