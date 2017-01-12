using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Areas.Catalog.Models.Delivery;
using Vtb24.Arms.Catalog.Models.Delivery;
using Vtb24.Arms.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using PartnerLocationStatus = Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.PartnerLocationStatus;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class DeliveryController : BaseController
    {
        private const int LOCATIONS_PAGE_SIZE = 100;
        private const int LOCATIONS_HISTORY_PAGE_SIZE = 10;
        private const int IMPORT_HISTORY_PAGE_SIZE = 30;

        public DeliveryController(IGiftShopManagement catalog, IGeoService geo, IAdminSecurityService security)
        {
            _catalog = catalog;
            _geo = geo;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IGeoService _geo;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login,
                                                          PermissionKeys.Catalog_Partners_DeliveryMatrix);

            var query = new DeliveryQueryModel { hidebinded = true };

            return RedirectToAction("List", "Delivery", query);            
        }

        [HttpGet]
        public ActionResult List(DeliveryQueryModel query)
        {
            var dropdown = GetDropdown(query.partnerId);

            if (dropdown == null)
            {
                return View("NoPartners");
            }
            
            var getLocationsOptions = new GetPartnerLocationsOptions
            {
                PartnerId = dropdown.SelectedPartnerId,
                SearchTerm = query.term
            };
            
            if (query.hidebinded)
            {
                getLocationsOptions.Statuses = new[]
                {
                    PartnerLocationStatus.NotBinded,
                    PartnerLocationStatus.UnknownKladr,
                    PartnerLocationStatus.DuplicateKladr
                };
            }

            var locations = _catalog.GetPartnerLocationsBindings(getLocationsOptions, PagingSettings.ByPage(query.page ?? 1, LOCATIONS_PAGE_SIZE));

            var model = new DeliveryModel
            {
                Dropdown = dropdown,
                Locations = locations.Select(PartnerLocationModel.Map)
                                     .OrderBy(l => l.LocationName)
                                     .ToArray(),
                Query = query,
                TotalPages = locations.TotalPages,
                ImportUrl = Url.Action("ImportPartnerMatrix", new { id = dropdown.SelectedPartnerId }),
                HistoryUrl = Url.Action("History", new { partnerId = dropdown.SelectedPartnerId }),
                UpdateBinding = new UpdateBindingModel()
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult History(int partnerId, int? page)
        {
            var partner = _catalog.GetUserPartnerInfoById(partnerId);

            if (partner == null)
            {
                return HttpNotFound();
            }

            var dropdown = GetDropdown(partnerId);

            if (dropdown == null)
            {
                return HttpNotFound();
            }

            var locationsHistory = _catalog.GetDeliveryLocationsHistory(partnerId, PagingSettings.ByPage(page ?? 1, LOCATIONS_HISTORY_PAGE_SIZE));
            
            var title = partner is CarrierInfo ? "Курьер" : "Поставщик";

            var model = new HistoryModel
            {
                Dropdown = dropdown,
                Title = string.Format("{0} \"{1}\": история изменений привязок", title, partner.Name),
                BindingsHistory = locationsHistory.Select(PartnerLocationHistoryModel.Map)
                                                  .OrderByDescending(h => h.DateTime)
                                                  .ToArray(),
                BackUrl = Url.Action("List", "Delivery", new DeliveryQueryModel { partnerId = partnerId, hidebinded = true }),
                TotalPages = locationsHistory.TotalPages,
                page = page
            };

            return View("History", model);
        }

        [HttpGet]
        public ActionResult ImportPartnerMatrix(int id, int? page)
        {
            var partner = _catalog.GetUserPartnerInfoById(id);

            if (partner == null)
            {
                return HttpNotFound();
            }

            var importTasks = _catalog.GetDeliveryRatesImportsHistory(id, PagingSettings.ByPage(page ?? 1, IMPORT_HISTORY_PAGE_SIZE));

            var title = partner is CarrierInfo ? "Курьер" : "Поставщик";

            var model = new PartnerMatrixImportModel
            {
                Title = string.Format("{0} \"{1}\": загрузка матрицы стоимости доставки", title, partner.Name),
                MenuId = "delivery",
                Id = partner.Id,
                Name = partner.Name,
                BackUrl = Url.Action("List", "Delivery", new DeliveryQueryModel { partnerId = id, hidebinded = true }),
                PostController = "Delivery",
                PostAction = "ImportPartnerMatrix",
                ImportTasks = importTasks.Select(PartnerMatrixImportTaskModel.Map).ToArray(),
                TotalPages = importTasks.TotalPages,
                page = page
            };

            return View("MatrixImport", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportPartnerMatrix(int id, HttpPostedFileBase file)
        {
            var supplier = _catalog.GetUserPartnerInfoById(id);

            if (supplier == null)
                throw new InvalidOperationException(string.Format("Поставщик не найден: {0}", id));

            file.ImportMatrixForPartner(_catalog, id, this);

            return RedirectToAction("ImportPartnerMatrix", "Delivery", new { id });
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult GetLocationByKladrCode(string kladrCode)
        {
            var location = _geo.GetLocationByKladr(kladrCode);

            return Json(MapForAutocomplete(location), JsonRequestBehavior.AllowGet);      
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Search(string term, SearchType type, string parentKladr = null)
        {
            GeoLocationType[] types = null;
            switch (type)
            {
                case SearchType.Region:
                    types = new[] {GeoLocationType.Region};
                    break;

                case SearchType.District:
                    types = new[] {GeoLocationType.District};
                    break;

                case SearchType.City:
                    types = new[] {GeoLocationType.City, GeoLocationType.Town};
                    break;
            }

            var options = new GeoLocationQuery
            {
                ParentKladrCode = string.IsNullOrEmpty(parentKladr) ? null : parentKladr, // TODO: у бэка проверка только на null. Жесть.
                SearchTerm = term,
                Types = types
            };

            var locations = _geo.Find(options, Vtb24.Site.Services.Models.PagingSettings.ByOffset(0, 20));

            return Json(locations.Select(MapForAutocomplete), JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBinding(UpdateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            try
            {
                _catalog.SetPartnerLocationBinding(model.Id, model.KladrCode);
            }
            catch (DeliveryLocationKladrAlreadyBindedException ex)
            {
                ModelState.AddModelError("KladrCode", ex.Text);
                return JsonErrors();
            }

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetBinding(int id)
        {
            _catalog.ResetPartnerLocationBinding(id);

            return JsonSuccess();
        }

        private static AutocompleteSuggestionContextModel MapForAutocomplete(GeoLocation location)
        {
            string label;
            if (location.Type == GeoLocationType.Region)
            {
                label = location.GetFullRegionName();
            }
            else if (location.Type == GeoLocationType.District || location.Type == GeoLocationType.City || string.IsNullOrWhiteSpace(location.DistrictName))
            {
                label = string.Format("{0}, {1}", location.GetFullName(), location.GetFullRegionName());
            }
            else
            {
                label = string.Format("{0}, {1}, {2}", location.GetFullName(), location.GetFullDistrictName(), location.GetFullRegionName());
            }

            return new AutocompleteSuggestionContextModel
            {
                selectionFullLabel = label,
                selectionLabel = location.GetFullName(),
                selectionValue = location.KladrCode,
                districtLabel = location.GetFullDistrictName(),
                districtValue = location.DistrictKladrCode,
                regionLabel = location.GetFullRegionName(),
                regionValue = location.RegionKladrCode,
                type = location.Type.ToString()
            };
        }

        private DropdownPartnerModel GetDropdown(int? partnerId)
        {
            var suppliers = _catalog.GetUserSuppliersInfo()
                                    .Where(s => s.Type != SupplierType.Online)
                                    .OrderBy(s => s.Name)
                                    .Select(s => new DropdownPartnerRowModel
                                    {
                                        Id = s.Id,
                                        Name = s.Name,
                                        Type = PartnerType.Supplier,
                                        IsSelected = s.Id == partnerId
                                    })
                                    .ToArray();

            var carriers = _catalog.GetUserCarriersInfo()
                                   .OrderBy(s => s.Name)
                                   .Select(s => new DropdownPartnerRowModel
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       Type = PartnerType.Carrier,
                                       IsSelected = s.Id == partnerId
                                   })
                                   .ToArray();

            var selectedSupplier = suppliers.FirstOrDefault(s => s.IsSelected);
            var selectedCarrier = carriers.FirstOrDefault(c => c.IsSelected);
            var selectedPartnerId = 0;

            if (selectedSupplier == null && selectedCarrier == null)
            {
                if (suppliers.Any())
                {
                    var supplier = suppliers.First();
                    selectedPartnerId = supplier.Id;
                    supplier.IsSelected = true;
                }
                else if (carriers.Any())
                {
                    var carrier = carriers.First();
                    selectedPartnerId = carrier.Id;
                    carrier.IsSelected = true;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (selectedSupplier != null)
                {
                    selectedPartnerId = selectedSupplier.Id;
                }
                if (selectedCarrier != null)
                {
                    selectedPartnerId = selectedCarrier.Id;
                }
            }

            return new DropdownPartnerModel
            {
                Carriers = carriers,
                Suppliers = suppliers,
                SelectedPartnerId = selectedPartnerId
            };
        }
    }
}
