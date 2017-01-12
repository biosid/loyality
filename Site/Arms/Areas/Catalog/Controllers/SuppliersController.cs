using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.Catalog.Models.Partners.Helpers;
using Vtb24.Arms.Catalog.Models.Shared.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Catalog.Models.Partners;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class SuppliersController : BaseController
    {
        public SuppliersController(IGiftShopManagement catalog, IAdminSecurityService security)
        {
            _catalog = catalog;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            var suppliers = _catalog.GetSuppliers().OrderBy(s => s.Name);

            var model = new SuppliersModel
            {
                Suppliers = suppliers.Select(SupplierModel.Map).ToArray(),
                Permissions = PartnersPermissionsModel.Map(_security)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login,
                                                          PermissionKeys.Catalog_Partners,
                                                          PermissionKeys.Catalog_Partners_Edit);

            var model = SupplierEditModel.Create();

            Hydrate(model);
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(SupplierEditModel model)
        {
            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                var id = _catalog.CreateSupplier(new Supplier
                {
                    Name = model.Name,
                    Type = model.Type.Map(),
                    Status = PartnerStatus.Disabled,
                    TrustLevel = model.TrustLevel.Map(),
                    CarrierId = model.CarrierId,
                    Description = model.Description,
                    Settings = new Dictionary<string, string>()
                });

                if (model.IsSettingsRequired())
                {
                    var supplier = _catalog.GetSupplierById(id);
                    if (supplier == null)
                        return RedirectToAction("Index", "Suppliers");

                    var editModel = SupplierEditModel.Map(supplier);
                    editModel.EditMode = PartnerEditMode.Setup;
                    editModel.Status = model.Status;
                    editModel.MapSettings(supplier.Settings);

                    Hydrate(editModel);
                    return View("Edit", editModel);
                }

                return RedirectToAction("Index", "Suppliers");
            }
            catch (PartnerNameAlreadyExistsException)
            {
                Hydrate(model);
                ModelState.AddModelError("Name", string.Format("Партнер с наименованием \"{0}\" уже существует", model.Name));

                return View("Edit", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var supplier = _catalog.GetSupplierById(id);
            if (supplier == null)
                return HttpNotFound();

            var model = SupplierEditModel.Map(supplier);
            model.MapSettings(supplier.Settings);

            Hydrate(model);
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(SupplierEditModel model)
        {
            var supplier = _catalog.GetSupplierById(model.SupplierId);
            if (supplier == null)
                return HttpNotFound();

            model.ValidateSettings(supplier.Settings, ModelState);

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                _catalog.UpdateSupplier(new Supplier
                {
                    Id = model.SupplierId,
                    Name = model.Name,
                    Type = model.Type.Map(),
                    Status = model.Status.Map(),
                    TrustLevel = model.TrustLevel.Map(),
                    CarrierId = model.CarrierId,
                    Description = model.Description,
                    Settings = model.Settings.MergeSettings(supplier.Settings)
                });

                return RedirectToAction("Index", "Suppliers");
            }
            catch (PartnerNameAlreadyExistsException)
            {
                Hydrate(model);
                ModelState.AddModelError("Name", string.Format("Партнер с наименованием \"{0}\" уже существует", model.Name));

                return View("Edit", model);
            }
        }

        private void Hydrate(SupplierEditModel model)
        {
            model.CarriersList = GetCarriersList(model.CarrierId).ToArray();
        }

        private IEnumerable<SelectListItem> GetCarriersList(int? carrierId)
        {
            yield return new SelectListItem
            {
                Selected = !carrierId.HasValue,
                Text = "- нет -",
                Value = ""
            };

            var carriers = _catalog.GetCarriers().OrderBy(p => p.Name);

            foreach (var carrier in carriers)
            {
                yield return new SelectListItem
                {
                    Selected = carrierId.HasValue && carrierId.Value == carrier.Id,
                    Text = carrier.MapName(),
                    Value = carrier.Id.ToString("d")
                };
            }
        }
    }
}
