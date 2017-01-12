using System;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class FilesController : Controller
    {
        [HttpGet]
        public FileResult Uploads(Guid id, string fileName)
        {
            var upload = SiteUploadsHelper.FindUpload(id);

            if (upload == null || upload.FileName != fileName)
            {
                throw new HttpException(404, "Файл не найден");
            }

            return File(upload.Path, MimeMapping.GetMimeMapping(upload.FileName), upload.FileName);
        }

    }
}
