using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Rapidsoft.VTB24.Reports.Site.Helpers;
using Rapidsoft.VTB24.Reports.Site.Models.PixelReports;
using Rapidsoft.VTB24.Reports.Site.Models.Shared;
using Rapidsoft.VTB24.Reports.Statistics;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;

namespace Rapidsoft.VTB24.Reports.Site.Controllers
{
    public class PixelReportsController : Controller
    {
        public PixelReportsController(IPixelReports pixelReports)
        {
            _pixelReports = pixelReports;
        }

        private readonly IPixelReports _pixelReports;

        [HttpGet]
        public ActionResult Index()
        {
            return View("List");
        }

        [HttpGet]
        public ActionResult GetList(int skip, int take)
        {
            var reports = _pixelReports.GetReportBriefs(skip, take);

            var model = new ReportsListModel
            {
                Reports = reports.Select(ToReportBriefModel).ToArray()
            };

            if (reports.Length == take)
            {
                model.NextSkip = skip + take;
            }

            return View("_List", model);
        }

        [HttpGet]
        public ActionResult NewQuery()
        {
            var now = DateTime.Now;
            var model = new ReportQueryModel
            {
                Pixel = PixelStrings[0],
                FromDate = new DateModel(now.AddMonths(-1)),
                ToDate = new DateModel(now.AddDays(-1))
            };

            return RedirectToAction("Query", "PixelReports", model);
        }

        [HttpGet]
        public ActionResult Query(ReportQueryModel query)
        {
            Hydrate(query);

            return View("Query", query);
        }

        [HttpPost]
        public ActionResult CreateReport(ReportQueryModel query)
        {
            if (!ModelState.IsValid)
            {
                Hydrate(query);

                return View("Query", query);
            }

            if (query.FromDate.Value > query.ToDate.Value)
            {
                var date = query.FromDate;
                query.FromDate = query.ToDate;
                query.ToDate = date;
            }

            _pixelReports.CreateReport(new ReportRequest
            {
                Pixel = query.Pixel,
                FromDate = DateTime.SpecifyKind(query.FromDate.Value, DateTimeKind.Utc),
                ToDate = DateTime.SpecifyKind(query.ToDate.Value, DateTimeKind.Utc)
            });

            return RedirectToAction("Index", "PixelReports");
        }

        [HttpGet]
        public ActionResult Csv(Guid id)
        {
            if (!_pixelReports.IsReady(id))
            {
                return HttpNotFound();
            }

            var fileName = String.Format("pixelreport-{0}.csv", string.Format("{0:yyyyMMddHHmm}", DateTime.Now));

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "text/csv";
            Response.Charset = "Windows-1251";

            using (var writer = new StreamWriter(Response.OutputStream, Encoding.GetEncoding(1251)))
            {
                _pixelReports.WriteReportCsv(id, writer);
            }

            Response.Flush();
            Response.End();

            return new EmptyResult();
        }

        private static readonly string[] PixelStrings = new[]
        {
            "/content/images/wbbcn.png",
            "/content/images/wbbcn.png?w=1",
            "/content/images/wbbcn.png?w=2",
            "/content/images/wbbcn.png?w=3",
            "/content/images/wbbcn.png?w=4",
            "/content/images/wbbcn.png?w=5",
            "/content/images/wbbcn.png?w=6",
            "/content/images/wbbcn.png?w=7"
        };

        private static IEnumerable<SelectListItem> GetPixels(string selectedPixel)
        {
            return PixelStrings.Select(s => new SelectListItem
            {
                Text = s,
                Value = s,
                Selected = s == selectedPixel
            });
        }

        private static void Hydrate(ReportQueryModel query)
        {
            query.Pixels = GetPixels(query.Pixel).ToArray();
        }

        private static ReportBriefModel ToReportBriefModel(ReportBrief brief)
        {
            return new ReportBriefModel
            {
                Id = brief.Status == ReportStatus.Ready ? brief.Id : (Guid?) null,
                CreateTimestamp = brief.CreateTimestamp,
                Status = brief.Status.GetEnumDescription(),
                Pixel = brief.Request.Pixel,
                FromDate = brief.Request.FromDate,
                ToDate = brief.Request.ToDate,
                ItemsCount =
                    brief.Status == ReportStatus.InProgress || brief.Status == ReportStatus.Ready
                        ? brief.ItemsCount
                        : (int?) null
            };
        }
    }
}
