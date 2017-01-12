using Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.Helpers;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;

namespace Rapidsoft.VTB24.Reports.Statistics.PixelReports
{
    internal static class MappingsFromDb
    {
        public static ReportBrief ToReportBrief(PixelReport original, int itemsCount)
        {
            return new ReportBrief
            {
                Id = original.Id,
                Request = ToReportRequest(original),
                Status = original.Status,
                CreateTimestamp = original.CreateTimestamp.FromDbDate(),
                ItemsCount = itemsCount
            };
        }

        public static ReportRequest ToReportRequest(PixelReport original)
        {
            return new ReportRequest
            {
                Pixel = original.Pixel,
                FromDate = original.FromDate.FromDbDate(),
                ToDate = original.ToDate.FromDbDate()
            };
        }

        public static PixelReportCsvItem ToPixelReportCsvItem(PixelReportItem original)
        {
            return new PixelReportCsvItem
            {
                Timestamp = original.Timestamp,
                IpAddress = original.IpAddress,
                Agent = original.Agent.Replace('+', ' ')
            };
        }
    }
}
