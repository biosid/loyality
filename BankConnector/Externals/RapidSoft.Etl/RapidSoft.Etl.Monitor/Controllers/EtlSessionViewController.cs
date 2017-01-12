namespace RapidSoft.Etl.Monitor.Controllers
{
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;

    using RapidSoft.Etl.Logging.Dumps;
    using RapidSoft.Etl.Monitor.Models;

    public class EtlSessionViewController : AsyncController
    {
        #region Fields

        private static readonly string _etlDumpMimeType = @"application/text";

        private const int pageSize = 100;

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult EtlSessionView(string packageId, string sessionId, string backUrl, int? page)
        {
            var model = SetDefaultModel(backUrl, page);
            var skip = (model.Page - 1) * pageSize;
            var dump = GetEtlDump(packageId, sessionId, pageSize, skip);

            if (dump.Sessions.Count == 1)
            {
                model.Session = dump.Sessions[0];
            }
            else
            {
                model.Session = new EtlSessionSummary();
                model.NotFound = true;
            }

            return View(model);
        }

        [HttpGet]
        public FileResult DownloadSessionDump(string packageId, string sessionId)
        {
            var dump = GetEtlDump(packageId, sessionId);

            var memoryStream = new MemoryStream();
            var writer = new XmlTextWriter(memoryStream, Encoding.UTF8);
            var ser = new XmlSerializer(typeof(EtlDump));
            ser.Serialize(writer, dump);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, _etlDumpMimeType)
            {
                FileDownloadName = GetEtlDumpFileName(dump)
            };
        }

        [HttpPost]
        public ActionResult Back(string backUrl)
        {
        	if (!string.IsNullOrEmpty(backUrl))
            {
                return Redirect(Server.UrlDecode(backUrl));
            }
        	return RedirectToAction("EtlSessionList", "EtlSessionList");
        }

    	#endregion

        #region Methods

        private static EtlSessionViewModel SetDefaultModel(string backUrl, int? page)
        {
            var model = new EtlSessionViewModel
                            {
                                BackUrl = backUrl,
                                Page = page ?? 1,
                                PageFromRequest = page.HasValue,
                                PageSize = pageSize
                            };

            return model;
        }

        private static string GetEtlDumpFileName(EtlDump dump)
        {
            var format = "EtlDump_{0:yyyyMMdd_HHmm}.xml";
            if (dump.Sessions.Count > 0)
            {
                return string.Format(format, dump.Sessions[0].StartDateTime);
            }
            else
            {
                return string.Format(format, dump.DumpDateTime);
            }
        }

        private static EtlDump GetEtlDump(string packageId, string sessionId, int? takeMessageCount = null, int? skipMessageCount = null)
        {
            var agent = SiteConfiguration.GetEtlAgent();
            var logParser = agent.GetEtlLogParser();

            var writer = new EtlDumpWriter(new EtlDumpSettings(takeMessageCount, skipMessageCount));
            writer.Write(packageId, sessionId, logParser);

            var dump = writer.GetDump();
            return dump;
        }

    	#endregion
    }
}