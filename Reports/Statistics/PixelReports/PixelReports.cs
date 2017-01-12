using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using LINQtoCSV;
using Rapidsoft.VTB24.Reports.Statistics.DataAccess;
using Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;

namespace Rapidsoft.VTB24.Reports.Statistics.PixelReports
{
    public class PixelReports : IPixelReports
    {
        private const int MAX_TAKE = 100;

        public ReportBrief[] GetReportBriefs(int skip, int take)
        {
            #region валидация

            if (skip < 0)
            {
                throw new ArgumentException("'skip' must be greater then or equal zero");
            }

            if (take <= 0)
            {
                throw new ArgumentException("'take' must be greater then zero");
            }

            if (take > MAX_TAKE)
            {
                throw new ArgumentException("'take' must bew less then or equal " + MAX_TAKE.ToString("D"));
            }

            #endregion

            using (var context = new StatisticsContext())
            {
                var reports = context.Reports
                                     .OrderByDescending(r => r.CreateTimestamp)
                                     .Skip(skip)
                                     .Take(take)
                                     .ToArray();

                return reports.AsEnumerable()
                              .Select(r => MappingsFromDb.ToReportBrief(r, GetItemsCount(context, r)))
                              .ToArray();
            }
        }

        public void CreateReport(ReportRequest request)
        {
            using (var context = new StatisticsContext())
            {
                var report = new PixelReport
                {
                    Id = Guid.NewGuid(),
                    Pixel = request.Pixel,
                    FromDate = request.FromDate.ToUniversalTime().Date,
                    ToDate = request.ToDate.ToUniversalTime().Date,
                    CreateTimestamp = DateTime.UtcNow,
                    Status = ReportStatus.New
                };

                if (report.FromDate > report.ToDate)
                {
                    var temp = report.FromDate;
                    report.FromDate = report.ToDate;
                    report.ToDate = temp;
                }

                context.Reports.Add(report);

                context.SaveChanges();
            }
        }

        public bool IsReady(Guid id)
        {
            using (var context = new StatisticsContext())
            {
                return context.Reports.Any(r => r.Id == id && r.Status == ReportStatus.Ready);
            }
        }

        public void WriteReportCsv(Guid id, TextWriter writer)
        {
            using (var context = new StatisticsContext())
            {
                var report = context.Reports.FirstOrDefault(r => r.Id == id && r.Status == ReportStatus.Ready);

                if (report == null)
                {
                    return;
                }

                var items = context.Entry(report)
                                   .Collection(r => r.Items)
                                   .Query()
                                   .OrderBy(i => i.Timestamp)
                                   .AsEnumerable()
                                   .Select(MappingsFromDb.ToPixelReportCsvItem);

                var csvContext = new CsvContext();
                csvContext.Write(items, writer, new CsvFileDescription
                {
                    FirstLineHasColumnNames = true,
                    SeparatorChar = ';',
                    FileCultureInfo = CultureInfo.GetCultureInfo("ru-ru"),
                    EnforceCsvColumnAttribute = true
                });
            }
        }

        private static int GetItemsCount(DbContext context, PixelReport report)
        {
            return context.Entry(report).Collection(r => r.Items).Query().Count();
        }
    }
}
