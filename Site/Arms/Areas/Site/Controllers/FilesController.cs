using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ElFinder;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Site.Controllers
{
    public class FilesController : Controller
    {
        public FilesController(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        private IDriver _filesDriver;
        private Connector _filesConnector;

        private IDriver FilesDriver
        {
            get { return _filesDriver ?? (_filesDriver = CreateDriver()); }
        }

        private Connector FilesConnector
        {
            get { return _filesConnector ?? (_filesConnector = new Connector(FilesDriver)); }
        }

        private static string FilesPathBase
        {
            get { return ConfigurationManager.AppSettings["content_files_virtual_directory"]; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login);

            return View();
        }

        public ActionResult Connector()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login);

            return FilesConnector.Process(Request);
        }

        [HttpGet]
        public ActionResult Thumbnails(string id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login);

            return FilesConnector.GetThumbnail(Request, Response, id);
        }

        [HttpGet]
        public ActionResult FileUrl(string id)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login);

            var filesPath = FilesPathBase;

            if (string.IsNullOrWhiteSpace(filesPath))
                throw new InvalidOperationException("Виртуальная директория для статических файлов не сконфигурирована");

            var fullPath = FilesDriver.ParsePath(id);
            var path = filesPath + "/" + fullPath.RelativePath.Replace('\\', '/');
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        private IDriver CreateDriver()
        {
            var path = ConfigurationManager.AppSettings["content_files_path"];
            var thumbnailsPath = ConfigurationManager.AppSettings["content_files_thumbnails_path"];
            var title = ConfigurationManager.AppSettings["content_files_root_title"];
            var maxUploadSizeStr = ConfigurationManager.AppSettings["content_files_max_upload_size_mb"];

            double maxUploadSize;
            if (string.IsNullOrEmpty(maxUploadSizeStr) || !double.TryParse(maxUploadSizeStr, out maxUploadSize))
                maxUploadSize = 1;

            var driver = new FileSystemDriver();
            var thumbsStorage = new DirectoryInfo(Server.MapPath(thumbnailsPath));
            driver.AddRoot(new Root(new DirectoryInfo(Server.MapPath(path)))
            {
                Alias = title,
                ThumbnailsStorage = thumbsStorage,
                MaxUploadSizeInMb = maxUploadSize,
                ThumbnailsUrl = "/site/files/thumbnails/"
            });

            return driver;
        }
    }
}
