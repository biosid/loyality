using System;

namespace Rapidsoft.VTB24.Reports.Etl
{
    public interface IEtlReportItemsBuilder<out TReportItem>
    {
        TReportItem[] Build(DateTime timestamp, DateTime fromDate, DateTime toDate, bool skipRowCountDiscrepancyCheck);
    }
}
