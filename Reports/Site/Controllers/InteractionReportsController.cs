using System;
using System.Web;
using System.Web.Mvc;
using Rapidsoft.VTB24.Reports.Etl;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;
using Rapidsoft.VTB24.Reports.Site.Models.InteractionReports;
using Rapidsoft.VTB24.Reports.Site.Models.Shared;

namespace Rapidsoft.VTB24.Reports.Site.Controllers
{
    public class InteractionReportsController : Controller
    {
        public InteractionReportsController(IEtlReports etlReports)
        {
            _etlReports = etlReports;
        }

        private readonly IEtlReports _etlReports;

        [HttpGet]
        public ActionResult Index()
        {
            var now = DateTime.Now;

            return RedirectToAction("Query", "InteractionReports", new ReportQueryModel
            {
                Type = InteractionType.LoyaltyRegistrations,
                FromDate = new DateModel(now.AddMonths(-1)),
                ToDate = new DateModel(now)
            });
        }

        [HttpGet]
        public ActionResult Query(ReportQueryModel query)
        {
            return View("Query", query);
        }

        [HttpGet]
        public ActionResult Report(ReportQueryModel query)
        {
            if (!ModelState.IsValid)
            {
                if (query.Print)
                {
                    throw new HttpException(400, "неверные параметры запроса");
                }

                return View("Query", query);
            }

            if (query.FromDate.Value > query.ToDate.Value)
            {
                var date = query.FromDate;
                query.FromDate = query.ToDate;
                query.ToDate = date;
            }

            try
            {
                return BuildReport(query);
            }
            catch (NotSupportedException)
            {
                ModelState.AddModelError("Type", "тип не поддерживается");
                return View("Query", query);
            }
        }

        private ActionResult BuildReport(ReportQueryModel query)
        {
            switch (query.Type)
            {
                case InteractionType.LoyaltyClientUpdates:
                    return BuildLoyaltyReport(query);

                case InteractionType.LoyaltyRegistrations:
                case InteractionType.Detachments:
                case InteractionType.Orders:
                    return BuildLoyaltyBankLoyaltyReport(query);

                case InteractionType.BankRegistrations:
                case InteractionType.Activations:
                case InteractionType.BankClientUpdates:
                case InteractionType.Accruals:
                case InteractionType.Audiences:
                case InteractionType.Messages:
                case InteractionType.LoginUpdates:
                case InteractionType.PasswordResets:
                case InteractionType.BankOffers:
                    return BuildBankLoyaltyReport(query);

                case InteractionType.PromoActions:
                    return BuildLoyaltyBankReport(query);
            }

            throw new NotSupportedException("InteractionType \"{0}\" is not supported");
        }

        private ActionResult BuildLoyaltyReport(ReportQueryModel query)
        {
            var report = _etlReports.LoyaltyReport(ToReportRequest(query));

            var viewName = GetViewName("Loyalty", IsEmpty(report), query.Print);

            return View(viewName, report);
        }

        private ActionResult BuildBankLoyaltyReport(ReportQueryModel query)
        {
            var report = _etlReports.BankLoyaltyReport(ToReportRequest(query));

            var viewName = GetViewName("BankLoyalty", IsEmpty(report), query.Print);

            return View(viewName, report);
        }

        private ActionResult BuildLoyaltyBankReport(ReportQueryModel query)
        {
            var report = _etlReports.LoyaltyBankReport(ToReportRequest(query));

            var viewName = GetViewName("LoyaltyBank", IsEmpty(report), query.Print);

            return View(viewName, report);
        }

        private ActionResult BuildLoyaltyBankLoyaltyReport(ReportQueryModel query)
        {
            var report = _etlReports.LoyaltyBankLoyaltyReport(ToReportRequest(query));

            var viewName = GetViewName("LoyaltyBankLoyalty", IsEmpty(report), query.Print);

            return View(viewName, report);
        }

        private static ReportRequest ToReportRequest(ReportQueryModel query)
        {
            return new ReportRequest
            {
                Type = query.Type,
                FromDate = query.FromDate.Value,
                ToDate = query.ToDate.Value,
                SkipRowCountDiscrepancyCheck = IsSkipRowCountDiscrepancyCheck(query.Type)
            };
        }

        private static bool IsSkipRowCountDiscrepancyCheck(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.LoyaltyClientUpdates:
                case InteractionType.Orders:
                    return true;
            }

            return false;
        }

        private static bool IsEmpty<TReportItem>(Report<TReportItem> report)
        {
            return report.Items == null || report.Items.Length == 0;
        }

        private static string GetViewName(string reportName, bool isEmpty, bool print)
        {
            return (isEmpty ? "Empty" : reportName) + (print ? "Print" : string.Empty);
        }
    }
}
