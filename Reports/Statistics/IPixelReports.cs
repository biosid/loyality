using System;
using System.IO;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;

namespace Rapidsoft.VTB24.Reports.Statistics
{
    public interface IPixelReports
    {
        ReportBrief[] GetReportBriefs(int skip, int take);

        void CreateReport(ReportRequest request);

        bool IsReady(Guid id);

        void WriteReportCsv(Guid id, TextWriter writer);
    }
}
