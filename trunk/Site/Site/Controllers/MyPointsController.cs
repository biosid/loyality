using System;
using System.Globalization;
using System.Web.Mvc;
using System.Linq;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.MyPoints;
using Vtb24.Site.Services;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing.Models;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    public class MyPointsController : BaseController
    {
        public const int PAGE_SIZE = 30;
        public const int RECENT_SIZE = 5;

        private static readonly DateTime LowestDate = new DateTime(1970, 1, 1);

        public MyPointsController(IClientService client)
        {
            _client = client;
        }

        private readonly IClientService _client;

        [HttpGet]
        public ActionResult Recent()
        {
            var balance = _client.GetBalance();

            var history = _client.GetOperationsHistory(LowestDate, DateTime.Now, PagingSettings.ByOffset(0, RECENT_SIZE));

            var model = new MyPointsModel
            {
                ClientBalance = balance,
                Operations = history.Select(OperationModel.Map).ToArray(),
                BonusTotal = history.Sum(i => i.Type == ProcessingOperationType.Deposit ? Math.Abs(i.Sum) : -Math.Abs(i.Sum)),
                ShowMonthFilter = false,
                TotalPages = 1,
                Page = 1
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Index(DateTimeRange range, int page = 1)
        {
            if ((range.Start.HasValue && range.Start.Value < LowestDate) ||
                (range.End.HasValue && range.End.Value < LowestDate) ||
                (range.Start.HasValue && range.Start.Value > DateTime.Now) ||
                (range.End.HasValue && range.End.Value > DateTime.Now))
            {
                return RedirectToAction("Index");
            }

            var actualRange = GetActualRange(range);
            var fromDate = actualRange.GetActualStartValue();
            var toDate = actualRange.GetActualEndValue();

            var history = _client.GetOperationsHistory(fromDate, toDate, PagingSettings.ByPage(page, PAGE_SIZE));

            if (!range.HasValues && history.TotalCount == 0)
            {
                return RedirectToAction("Recent");
            }

            var balance = _client.GetBalance();

            var model = new MyPointsModel
            {
                Operations = history.Select(OperationModel.Map).ToArray(),
                BonusTotal = history.TotalIncome - history.TotalOutcome,
                StartDate = range.Start,
                EndDate = range.End,
                DateLabel = fromDate.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")),
                ShowMonthFilter = fromDate.Month == toDate.Month,
                TotalPages = history.TotalPages,
                Page = page
            };

            if (fromDate == LowestDate)
            {
                model.PrevMonthUrl = "";
            }
            else
            {
                var prevMonth = DateTimeRange.GetMonthRange(fromDate.AddMonths(-1));
                model.PrevMonthUrl = Url.Action("Index",
                                                new
                                                {
                                                    from = prevMonth.GetActualStartValue().ToShortDateString(),
                                                    to = prevMonth.GetActualEndValue().ToShortDateString()
                                                });
            }

            if (fromDate.Month == DateTime.Now.Month &&
                fromDate.Year == DateTime.Now.Year)
            {
                model.NextMonthUrl = "";
            }
            else
            {
                var nextMonth = DateTimeRange.GetMonthRange(fromDate.AddMonths(1));

                model.NextMonthUrl = Url.Action("Index",
                                                new
                                                {
                                                    from = nextMonth.GetActualStartValue().ToShortDateString(),
                                                    to = nextMonth.GetActualEndValue().ToShortDateString()
                                                });
            }
            model.ClientBalance = balance;

            return View("Index", model);
        }

        private DateTimeRange GetActualRange(DateTimeRange range)
        {
            DateTime fromDate;
            if (range.Start.HasValue)
            {
                fromDate = range.Start.Value;
            }
            else
            {
                fromDate = range.End.HasValue
                               ? range.End.Value.AddMonths(-1)
                               : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            var toDate = range.End.HasValue ? range.End.Value : DateTime.Now;

            if (toDate == fromDate)
            {
                toDate = toDate.Add(new TimeSpan(0, 23, 59, 59, 999));
            }

            return new DateTimeRange(fromDate, toDate);
        }
    }
}
