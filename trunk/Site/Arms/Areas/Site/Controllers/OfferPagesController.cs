using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Site.Models.Pages;
using Vtb24.Site.Content.History.Models;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Exceptions;
using Vtb24.Site.Content.Pages.Models.Inputs;

namespace Vtb24.Arms.Site.Controllers
{
    [Authorize]
    public class OfferPagesController : BaseController
    {
        public OfferPagesController(IAdminPagesManagement pages, IGiftShopManagement giftShop, IAdminSecurityService security)
        {
            _pages = pages;
            _giftShop = giftShop;
            _security = security;
        }
        
        private const int PAGE_SIZE = 50;

        private readonly IAdminPagesManagement _pages;
        private readonly IGiftShopManagement _giftShop;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("List", "OfferPages");
        }

        [HttpGet]
        public ActionResult List(OfferPagesQueryModel query)
        {
            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);

            var result = _pages.GetAllOfferPages(paging);

            var model = new OfferPagesModel
            {
                Pages = result.Select(r =>
                {
                    var partnerId = int.Parse(r.ExternalId);

                    return OfferPageModel.Map(r, partnerId);
                }).ToArray(),
                TotalPages = result.TotalPages,
                Query = query
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OfferPageEditModel model)
        {
            if (ModelState.IsValid && string.IsNullOrWhiteSpace(model.Data.Content))
            {
                ModelState.AddModelError("Data.Content", "Необходимо ввести содержимое");
            }

            if (ModelState.IsValid)
            {
                var options = new CreateOfferPageOptions { Data = model.Data, PartnerId = model.PartnerId };

                try
                {
                    _pages.CreateOfferPage(options);
                }
                catch (PageUrlExistsException)
                {
                    ModelState.AddModelError("Data.Url", "Оферта с данным URL уже существует");
                }
                catch (OfferPageExistsException)
                {
                    ModelState.AddModelError("PartnerId", "У данного партнера уже существует оферта");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Partners = GetPartnersWithoutOffers();

                return View("Edit", model);
            }
            return RedirectToAction("List", "OfferPages", model.QueryModel);
        }

        [HttpGet]
        public ActionResult Create(string query)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Site_Login, PermissionKeys.Site_Page);

            var model = new OfferPageEditModel
            {
                IsNewPage = true,
                Data = new PageData(),
                Versions = new Snapshot[0],
                Query = query,
                Partners = GetPartnersWithoutOffers()
            };

            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Edit(int partnerId, Guid? versionId, string query)
        {
            var page = _pages.GetOfferPageByPartnerId(partnerId, true);
            if (page == null)
                return HttpNotFound();

            var snapshot = versionId.HasValue
                               ? page.History.Versions.FirstOrDefault(s => s.Id == versionId.Value)
                               : page.CurrentVersion;
            if (snapshot == null)
                return HttpNotFound();

            var partner = _giftShop.GetSupplierInfoById(partnerId);

            var model = new OfferPageEditModel
            {
                IsNewPage = false,
                PartnerId = partnerId,
                PartnerName = partner.Name,
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
        public ActionResult Edit(OfferPageEditModel model)
        {
            if (ModelState.IsValid && string.IsNullOrWhiteSpace(model.Data.Content))
            {
                ModelState.AddModelError("Data.Content", "Необходимо ввести содержимое");
            }

            if (ModelState.IsValid)
            {
                var options = new UpdateOfferPageOptions { PartnerId = model.PartnerId, Data = model.Data };

                try
                {
                    _pages.UpdateOfferPage(options);
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
            {
                model.PartnerName = _giftShop.GetSupplierInfoById(model.PartnerId).Name;

                return View("Edit", model);
            }
            return RedirectToAction("List", "OfferPages", model.QueryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(ChangeOfferPageStatusModel model)
        {
            _pages.ChangeOfferPagesStatus(model.PartnerIds, model.Status);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int partnerId)
        {
            _pages.RemoveOfferPage(partnerId);

            return JsonSuccess();
        }

        private SelectListItem[] GetPartnersWithoutOffers(int? partnerId = null)
        {
            var allPartners = _giftShop.GetSuppliersInfo();

            var partners = allPartners.Where(p => !_pages.IsPartnerGotOfferPage(p.Id)).ToArray();

            return partners.Select(p => new SelectListItem
                                    {
                                        Text = p.Name,
                                        Value = p.Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = partnerId.HasValue && p.Id == partnerId.Value
                                    }).ToArray();
        }
    }
}