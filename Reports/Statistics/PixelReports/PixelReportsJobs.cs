using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Rapidsoft.VTB24.Reports.IisLogs;
using Rapidsoft.VTB24.Reports.IisLogs.Scanner.Models;
using Rapidsoft.VTB24.Reports.Statistics.DataAccess;
using Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.Helpers;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;
using log4net;

namespace Rapidsoft.VTB24.Reports.Statistics.PixelReports
{
    public class PixelReportsJobs : IIisLogsJobs<ReportItem>
    {
        private const string REGEX_ESCAPE_CHARS = @".$^{[(|)*+\";
        private const string REGEX_BEGIN = @"^([0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}) [0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3} [A-Z]+ ";
        private const string REGEX_END = @" [0-9]+ [^ ]+ ([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}) ([^ ]+)";

        public PixelReportsJobs(IIisLogsScanner scanner)
        {
            _scanner = scanner;
            CleanupJob();
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(PixelReportsJobs));
        private readonly IIisLogsScanner _scanner;

        private Guid _reportId;

        public IEnumerable<ReportItem> CurrentJobItems { get; private set; }

        public void PeekJob()
        {
            PixelReport pixelReport;

            using (var context = new StatisticsContext())
            {
                pixelReport = context.Reports
                                     .Where(r => r.Status == ReportStatus.New)
                                     .OrderBy(r => r.CreateTimestamp)
                                     .FirstOrDefault();
            }

            if (pixelReport != null)
            {
                SetupJob(pixelReport);
            }
            else
            {
                CleanupJob();
            }
        }

        public void NotifyJobStarted()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var report = GetPixelReport(context, ReportStatus.New);

                report.Status = ReportStatus.InProgress;
                report.StartTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }
        }

        public void NotifyJobCancelled()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var report = GetPixelReport(context, ReportStatus.InProgress);

                context.Entry(report).Collection(r => r.Items).Load();
                context.ReportItems.RemoveRange(report.Items);

                report.Status = ReportStatus.New;
                report.FinishTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }

            CleanupJob();
        }

        public void NotifyJobSucceeded()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var report = GetPixelReport(context, ReportStatus.InProgress);

                report.Status = ReportStatus.Ready;
                report.FinishTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }

            CleanupJob();
        }

        public void NotifyJobFailed()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var report = GetPixelReport(context, ReportStatus.InProgress);

                report.Status = ReportStatus.Error;
                report.FinishTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }

            CleanupJob();
        }

        public void SaveBatch(ReportItem[] batch)
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var report = GetPixelReport(context, ReportStatus.InProgress);

                if (batch.Min(i => i.Timestamp) < report.FromDate.FromDbDate().Date)
                {
                    throw new ArgumentException("таймстэмп элемента отчета не может быть меньше FromDate");
                }
                if (batch.Max(i => i.Timestamp) > report.ToDate.FromDbDate().Date.AddTicks(TimeSpan.TicksPerDay - 1))
                {
                    throw new ArgumentException("таймстэмп элемента отчета не может быть больше ToDate");
                }

                var reportItems = batch.Select(i => new PixelReportItem
                {
                    Id = Guid.NewGuid(),
                    PixelReportId = _reportId,
                    Timestamp = i.Timestamp,
                    IpAddress = i.IpAddress,
                    Agent = i.Agent
                });

                context.ReportItems.AddRange(reportItems);
                context.SaveChanges();
            }
        }

        private void SetupJob(PixelReport pixelReport)
        {
            _reportId = pixelReport.Id;

            var pixelRegexString = BuildPixelRegexString(pixelReport.Pixel);

            var testRegex = new Regex(pixelRegexString);

            var matchRegex = new Regex(REGEX_BEGIN + pixelRegexString + REGEX_END);

            var fromDate = pixelReport.FromDate.FromDbDate();
            var toDate = pixelReport.ToDate.FromDbDate();

            CurrentJobItems = _scanner.ScanLogs(fromDate, toDate, testRegex)
                                      .Select(line => ToReportItem(line, matchRegex));
        }

        private void CleanupJob()
        {
            _reportId = Guid.Empty;
            CurrentJobItems = null;
        }

        private void AssertJob()
        {
            if (_reportId == Guid.Empty)
            {
                throw new InvalidOperationException("нет отчета для обработки");
            }
        }

        private PixelReport GetPixelReport(StatisticsContext context, ReportStatus allowedStatus)
        {
            var report = context.Reports.FirstOrDefault(r => r.Id == _reportId);

            if (report == null)
            {
                throw new ArgumentException("не найден отчет с id = " + _reportId);
            }

            if (report.Status != allowedStatus)
            {
                throw new InvalidOperationException(
                    string.Format("отчет с id = {0} должен быть в статусе {1}, текущий статус {2}",
                                  _reportId, allowedStatus, report.Status));
            }

            return report;
        }

        private static string BuildPixelRegexString(string pixel)
        {
            pixel = new string(pixel.SelectMany(c => REGEX_ESCAPE_CHARS.Contains(c)
                                                         ? new[] { '\\', c }
                                                         : new[] { c })
                                    .ToArray());

            var queryIndex = pixel.IndexOf('?');
            if (queryIndex == -1)
            {
                pixel += " -";
            }
            else
            {
                pixel = pixel.Substring(0, queryIndex) + " " + pixel.Substring(queryIndex + 1);
            }

            return pixel;
        }

        private ReportItem ToReportItem(IisLogLine logLine, Regex regex)
        {
            if (logLine == null)
            {
                return null;
            }

            var match = regex.Match(logLine.Line);

            if (!match.Success || match.Groups.Count != 4)
            {
                return null;
            }

            var timestampString = match.Groups[1].Value;
            var ipAddress = match.Groups[2].Value;
            var agent = match.Groups[3].Value;

            DateTime timestamp;
            if (!DateTime.TryParseExact(timestampString,
                                        "yyyy-MM-dd HH:mm:ss",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                        out timestamp))
            {
                _log.ErrorFormat("{0}:{1}: не удалось распознать таймстэмп: {2}", logLine.FileName, logLine.LineNumber, timestampString);
                return null;
            }

            var reportItem = new ReportItem
            {
                Timestamp = timestamp,
                IpAddress = ipAddress,
                Agent = agent.Replace('+', ' ')
            };

            return reportItem;
        }
    }
}
