using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LINQtoCSV;
using Rapidsoft.VTB24.Reports.IisLogs;
using Rapidsoft.VTB24.Reports.IisLogs.Scanner.Models;
using Rapidsoft.VTB24.Reports.Statistics.DataAccess;
using Rapidsoft.VTB24.Reports.Statistics.Entities.ProductViewEvents;
using Rapidsoft.VTB24.Reports.Statistics.Helpers;
using Rapidsoft.VTB24.Reports.Statistics.Models.ProductViewEvents;
using Rapidsoft.VTB24.Reports.WcfClients.CatalogAdmin;
using log4net;

namespace Rapidsoft.VTB24.Reports.Statistics.ProductViewEvents
{
    public class ProductViewEventsJobs : IIisLogsJobs<ViewEvent>
    {
        private const int MAX_ATTEMPTS_PER_DAY = 3;

        private const string TEST_REGEX = "/catalog/product";
        private const string MATCH_REGEX = @"^([0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}) [0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3} [A-Z]+ /catalog/product/([^ ]+) [^ ]+ [0-9]+ ([^ ]+) ([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}) ([^ ]+)";

        private static readonly CsvFileDescription EventsFileCsvDesc = new CsvFileDescription
        {
            FirstLineHasColumnNames = false,
            EnforceCsvColumnAttribute = true,
            SeparatorChar = ';'
        };

        public ProductViewEventsJobs(IIisLogsScanner scanner)
        {
            _scanner = scanner;

            Directory.CreateDirectory(_eventsFilesDirName);

            CleanupJob();
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(ProductViewEventsJobs));
        private readonly IIisLogsScanner _scanner;
        private readonly Regex _testRegex = new Regex(TEST_REGEX, RegexOptions.IgnoreCase);
        private readonly Regex _matchRegex = new Regex(MATCH_REGEX, RegexOptions.IgnoreCase);
        private readonly string _eventsFilesDirName = ConfigurationManager.AppSettings["vtb24:stat:views_files_path"];

        private readonly HashSet<string> _guestViews = new HashSet<string>();
        private readonly HashSet<string> _clientViews = new HashSet<string>();
        private readonly List<string> _views = new List<string>();

        private Guid _eventsDayId;
        private string _eventsFileName;

        public IEnumerable<ViewEvent> CurrentJobItems { get; private set; }

        public void PeekJob()
        {
            var now = DateTime.UtcNow;

            ViewEventsDay eventsDay;

            using (var context = new StatisticsContext())
            {
                eventsDay = context.ViewEventsDays
                                   .Where(d => d.Status == ViewEventsDayStatus.Ready)
                                   .OrderByDescending(d => d.Date)
                                   .FirstOrDefault();
            }

            if (eventsDay == null)
            {
                if (now.TimeOfDay >= TimeSpan.FromTicks(TimeSpan.TicksPerHour))
                {
                    SetupJob(now.Date.AddTicks(-TimeSpan.TicksPerDay));
                }
                else
                {
                    CleanupJob();
                }
            }
            else
            {
                var lastDate = eventsDay.Date.FromDbDate().Date;

                if (lastDate < now.AddTicks(-(2 * TimeSpan.TicksPerDay + TimeSpan.TicksPerHour)))
                {
                    SetupJob(lastDate.AddTicks(TimeSpan.TicksPerDay));
                }
                else
                {
                    CleanupJob();
                }
            }
        }

        public void NotifyJobStarted()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var eventsDay = GetEventsDay(context, ViewEventsDayStatus.New);

                eventsDay.Status = ViewEventsDayStatus.InProgress;
                eventsDay.StartTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }
        }

        public void NotifyJobCancelled()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var eventsDay = GetEventsDay(context, ViewEventsDayStatus.InProgress);

                context.ViewEventsDays.Remove(eventsDay);

                context.SaveChanges();
            }
        }

        public void NotifyJobSucceeded()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var eventsDay = GetEventsDay(context, ViewEventsDayStatus.InProgress);

                var groupedViews =
                    _views.GroupBy(v => v)
                          .Select(g => new KeyValuePairOfstringint
                          {
                              key = g.Key,
                              value = g.Count()
                          })
                          .ToArray();

                eventsDay.Status = ViewEventsDayStatus.Ready;
                eventsDay.Count = _views.Count;
                eventsDay.KeysCount = groupedViews.Length;

                SendResults(eventsDay.Date, groupedViews);

                context.SaveChanges();
            }
        }

        public void NotifyJobFailed()
        {
            AssertJob();

            using (var context = new StatisticsContext())
            {
                var eventsDay = GetEventsDay(context, ViewEventsDayStatus.InProgress);

                eventsDay.Status = ViewEventsDayStatus.Error;
                eventsDay.FinishTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }
        }

        public void SaveBatch(ViewEvent[] batch)
        {
            AssertJob();

            foreach (var view in batch.Where(v => v.Login != null
                                                      ? !_clientViews.Contains(v.ProductId + v.Login)
                                                      : !_guestViews.Contains(v.ProductId + v.Session)))
            {
                if (view.Login != null)
                {
                    _clientViews.Add(view.ProductId + view.Login);
                }
                else
                {
                    _guestViews.Add(view.ProductId + view.Session);
                }

                _views.Add(view.ProductId);
            }

            AppendViewsToFile(batch);
        }

        private void SetupJob(DateTime date)
        {
            using (var context = new StatisticsContext())
            {
                var count = context.ViewEventsDays.Count(d => d.Date == date);

                if (count >= MAX_ATTEMPTS_PER_DAY)
                {
                    _log.ErrorFormat("за {0} достигнуто максимальное число попыток обработки ({1})",
                                     date.ToString("dd.MM.yyyy"), MAX_ATTEMPTS_PER_DAY);
                    CleanupJob();
                    return;
                }

                if (count > 0)
                {
                    _log.WarnFormat("обнаружено {0} попытка(и) обработки данных за {1}",
                                    count, date.ToString("dd.MM.yyyy"));
                }

                var eventsDay = new ViewEventsDay
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Status = ViewEventsDayStatus.New,
                    Count = 0,
                    KeysCount = 0
                };

                context.ViewEventsDays.Add(eventsDay);
                context.SaveChanges();

                _eventsDayId = eventsDay.Id;
            }

            CreateViewsFile(date);

            _guestViews.Clear();
            _clientViews.Clear();
            _views.Clear();

            CurrentJobItems = _scanner.ScanLogs(date, date, _testRegex)
                                      .Select(ToProductViewEvent);
        }

        private void CleanupJob()
        {
            _eventsDayId = Guid.Empty;
            CurrentJobItems = null;
        }

        private void AssertJob()
        {
            if (_eventsDayId == Guid.Empty)
            {
                throw new InvalidOperationException("нет данных для обработки");
            }
        }
        private ViewEventsDay GetEventsDay(StatisticsContext context, ViewEventsDayStatus allowedStatus)
        {
            var eventsDay = context.ViewEventsDays.FirstOrDefault(d => d.Id == _eventsDayId);

            if (eventsDay == null)
            {
                throw new ArgumentException("не найдены данные с id = " + _eventsDayId);
            }

            if (eventsDay.Status != allowedStatus)
            {
                throw new InvalidOperationException(
                    string.Format("ошибка обработки данных с id = {0}: должна быть в статусе {1}, текущий статус {2}",
                                  _eventsDayId, allowedStatus, eventsDay.Status));
            }

            return eventsDay;
        }

        private ViewEvent ToProductViewEvent(IisLogLine line)
        {
            if (line == null)
            {
                return null;
            }

            var match = _matchRegex.Match(line.Line);

            if (!match.Success || match.Groups.Count != 6)
            {
                return null;
            }

            var timestampString = match.Groups[1].Value;
            var productId = match.Groups[2].Value.Replace('+', ' ').Trim().ToLower();

            if (string.IsNullOrWhiteSpace(productId) || productId.Length > 256)
            {
                _log.WarnFormat("{0}:{1}: некорректное значение ProductId: {2}", line.FileName, line.LineNumber, productId);
                return null;
            }

            var login = match.Groups[3].Value;
            var ipAddress = match.Groups[4].Value;
            var userAgent = match.Groups[5].Value;

            DateTime timestamp;
            if (!DateTime.TryParseExact(timestampString,
                                        "yyyy-MM-dd HH:mm:ss",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                        out timestamp))
            {
                _log.ErrorFormat("{0}:{1}: не удалось распознать таймстэмп: {2}", line.FileName, line.LineNumber, timestampString);
                return null;
            }

            string session = null;

            if (login == "-")
            {
                login = null;
                session = ipAddress + userAgent;
            }

            return new ViewEvent
            {
                Timestamp = timestamp,
                ProductId = productId,
                Login = login,
                Session = session
            };
        }

        private void SendResults(DateTime date, KeyValuePairOfstringint[] views)
        {
            var userId = ConfigurationManager.AppSettings["vtb24:vtb_system_user"];

            using (var client = new CatalogAdminServiceClient())
            {
                var response = client.SaveProductViewsForDay(date, views, userId);

                if (!response.Success)
                {
                    throw new Exception("ошибка при обращении к каталогу: [" + response.ResultCode + "] - " + response.ResultDescription);
                }
            }
        }

        private void CreateViewsFile(DateTime date)
        {
            _eventsFileName = Path.Combine(
                _eventsFilesDirName,
                date.ToString("yyyy-MM-dd") + "_" + _eventsDayId.ToString("D") + ".csv");

            using (File.Create(_eventsFileName)) {}
        }

        private void AppendViewsToFile(IEnumerable<ViewEvent> viewEvents)
        {
            var csvContext = new CsvContext();

            using (var writer = new StreamWriter(_eventsFileName, true))
            {
                csvContext.Write(viewEvents, writer, EventsFileCsvDesc);
            }
        }
    }
}
