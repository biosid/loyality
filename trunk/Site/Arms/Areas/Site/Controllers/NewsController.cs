using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Site.Models.News;
using Vtb24.Arms.Infrastructure;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.News;
using Vtb24.Site.Content.News.Management.Models.Inputs;

namespace Vtb24.Arms.Site.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {
        private const int MAX_NEWS_COUNT = 50;

        public NewsController(INewsManagement news, IAdminSecurityService security)
        {
            _news = news;
            _security = security;
        }

        private readonly INewsManagement _news;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index(NewsModelListQueryModel queryModel)
        {
            var model = new NewsModel {QueryModel = queryModel};

            var getNewsFilter = new GetNewsMessagesFilter
            {
                From = queryModel.from,
                To = queryModel.to,
                Keyword = queryModel.keyword,
                IncludeUnpublished = !queryModel.hideunpublished
            };

            var newsList = _news.GetNewsMessages(getNewsFilter, PagingSettings.ByOffset(0, MAX_NEWS_COUNT))
                .Select(NewsModelListItem.Map)
                .ToArray();

            model.NewsMessages = newsList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(long[] ids, bool status)
        {
            _news.Publish(ids, status);

            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult Create()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_News);

            var model = new EditNewsMessageModel
            {
                Status = NewsModelStatus.New,
                Priority = 1
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditNewsMessageModel model, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ModelState.AddModelError("PictureUrl", "Необходимо указать картинку");

                return View("Edit", model);
            }

            var pictureUrl = LoadFile(file);
            if (string.IsNullOrEmpty(pictureUrl))
            {
                ModelState.AddModelError("PictureUrl", "Ошибка при загрузке картинки");

                return View("Edit", model);
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            if (model.EndDate.HasValue && 
                model.EndDate.Value == model.StartDate.Value)
            {
                model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, 23, 59, 59);
            }

            _news.Create(new UpdateNewsMessageOption
            {
                Description = model.Description,
                Url = model.Url,
                Title = model.Title,
                IsPublished = model.IsPublished,
                Priority = model.Priority,
                StartDate = model.StartDate.Value,
                EndDate = model.EndDate,
                Segment = model.Segment,
                Picture = pictureUrl
            });

            return RedirectToAction("Index", "News");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var message = _news.GetById(id);
            var history = _news.GetAllHistoryById(id);

            var model = EditNewsMessageModel.Map (message, history);
            model.Status = NewsModelStatus.Edit;

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            _news.Delete(id);

            return JsonSuccess(new { url=Url.Action("Index", "News") });
        }

        [HttpGet]
        public ActionResult EditVersion(string snapshotId)
        {
            var snapshot = _news.GetFromHistoryBySnapshotId(snapshotId);
            var history = _news.GetAllHistoryById(snapshot.Entity.Id);

            var model = EditNewsMessageModel.Map (snapshot, history);
            model.Status = NewsModelStatus.History;
            
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditNewsMessageModel model, HttpPostedFileBase file)
        {
            var pictureUrl = model.PictureUrl;
            if (file == null && string.IsNullOrEmpty(model.PictureUrl))
            {
                ModelState.AddModelError("PictureUrl", "Необходимо указать картинку");
            }
            if (file != null)
            {
                pictureUrl = LoadFile(file);
                if (string.IsNullOrEmpty(pictureUrl))
                {
                    ModelState.AddModelError("PictureUrl", "Ошибка при загрузке картинки");
                }
            }

            if (!ModelState.IsValid)
            {
                var history = _news.GetAllHistoryById(model.Id);

                model.History = history.Select (NewsMessageHistoryModel.Map)
                                 .OrderByDescending (h => h.CreationDate)
                                 .ToArray ();
                
                if (model.History.Length > 0)
                {
                    model.History.First ().IsLastVersion = true;
                }

                if (model.Status == NewsModelStatus.History)
                {
                    var snapshot = _news.GetFromHistoryBySnapshotId (model.SnapshotId);
                    model.CreationDate = snapshot.CreationDate;
                }

                return View("Edit", model);
            }

            if (model.EndDate.HasValue &&
                model.EndDate.Value == model.StartDate.Value)
            {
                model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, 23, 59, 59);
            }

            var options = new UpdateNewsMessageOption
            {
                Description = model.Description,
                Url = model.Url,
                Title = model.Title,
                IsPublished = model.IsPublished,
                Priority = model.Priority,
                StartDate = model.StartDate.Value,
                EndDate = model.EndDate,
                Segment = model.Segment,
                Picture = pictureUrl
            };

            _news.Edit(model.Id, options);

            return RedirectToAction("Index", "News");
        }

        private static string LoadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString("N") + (string.IsNullOrEmpty(extension) ? "" : extension);
                var path = ConfigurationManager.AppSettings["content_pictures_save_path"];

                var fileFullName = Path.Combine(path, fileName);
                file.SaveAs(fileFullName);

                return fileName;
            }

            return string.Empty;
        }
    }
}
