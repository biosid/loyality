using System;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Site.Models.Pages;
using Vtb24.Arms.Infrastructure;
using Vtb24.Site.Content.History.Models;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Exceptions;
using Vtb24.Site.Content.Pages.Models.Inputs;

namespace Vtb24.Arms.Site.Controllers
{
    [Authorize]
    public class PlainPagesController : BaseController
    {
        public PlainPagesController(IAdminPagesManagement pages, IAdminSecurityService security)
        {
            _pages = pages;
            _security = security;
        }

        private const int PAGE_SIZE = 50;

        private readonly IAdminPagesManagement _pages;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("List", "PlainPages");
        }

        [HttpGet]
        public ActionResult List(PlainPagesQueryModel query)
        {
            var options = new GetPlainPagesOptions
            {
                SortOrder = PagesSortOrder.UrlAsc,
                IsBuiltin = query.hide_builtin ? false : (bool?) null
            };
            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);

            var result = _pages.GetAllPlainPages(options, paging);

            var model = new PlainPagesModel
            {
                Pages = result.Select(PlainPageModel.Map).ToArray(),
                TotalPages = result.TotalPages,
                Query = query
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string query)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            var model = new PlainPageEditModel
            {
                IsNewPage = true,
                IsBuiltin = false,
                Data = new PageData(),
                Versions = new Snapshot[0],
                Query = query
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlainPageEditModel model)
        {
            if (ModelState.IsValid && string.IsNullOrWhiteSpace(model.Data.Content))
            {
                ModelState.AddModelError("Data.Content", "Необходимо ввести содержимое");
            }

            if (ModelState.IsValid)
            {
                var options = new CreatePlainPageOptions { Data = model.Data };

                try
                {
                    _pages.CreatePlainPage(options);
                }
                catch (PageUrlExistsException)
                {
                    ModelState.AddModelError("Data.Url", "Страница с данным URL уже существует");
                }
            }

            if (!ModelState.IsValid)
                return View("Edit", model);

            return RedirectToAction("List", "PlainPages", model.QueryModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id, Guid? versionId, string query)
        {
            var options = new GetPlainPagesOptions { LoadFullHistory = true };
            var page = _pages.GetPlainPageById(id, options);
            if (page == null)
                return HttpNotFound();

            var snapshot = versionId.HasValue
                               ? page.History.Versions.FirstOrDefault(s => s.Id == versionId.Value)
                               : page.CurrentVersion;
            if (snapshot == null)
                return HttpNotFound();

            var model = new PlainPageEditModel
            {
                IsNewPage = false,
                IsBuiltin = page.IsBuiltin,
                Id = id,
                CurrentVersionId = page.CurrentVersion.Id,
                ThisVersionId = snapshot.Id,
                Data = snapshot.Data,
                Versions = page.History
                               .Versions
                               .Cast<Snapshot>()
                               .OrderByDescending(s => s.When).ToArray(),
                Query = query
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlainPageEditModel model)
        {
            if (ModelState.IsValid && !model.IsBuiltin && string.IsNullOrWhiteSpace(model.Data.Content))
            {
                ModelState.AddModelError("Data.Content", "Необходимо ввести содержимое");
            }

            if (ModelState.IsValid)
            {
                var options = new UpdatePlainPageOptions { Id = model.Id, Data = model.Data };

                try
                {
                    _pages.UpdatePlainPage(options);
                }
                catch (PageNotFoundException)
                {
                    return HttpNotFound();
                }
                catch (PageUrlExistsException)
                {
                    ModelState.AddModelError("Data.Url", "Страница с данным URL уже существует");
                }
            }

            if (!ModelState.IsValid)
                return View("Edit", model);

            return RedirectToAction("List", "PlainPages", model.QueryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(ChangePlainPageStatusModel model)
        {
            _pages.ChangePlainPagesStatus(model.Ids, model.Status);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _pages.RemovePlainPage(id);

            return JsonSuccess();
        }
    }
}
