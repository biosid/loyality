using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.Catalog.Models.Partners.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Catalog.Models.Partners;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class CarriersController : BaseController
    {
        public CarriersController(IGiftShopManagement catalog, IAdminSecurityService security)
        {
            _catalog = catalog;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            var carriers = _catalog.GetCarriers().OrderBy(c => c.Name);

            var model = new CarriersModel
            {
                Carriers = carriers.Select(CarrierModel.Map).ToArray(),
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

            var model = CarrierEditModel.Create();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CarrierEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            try
            {
                var id = _catalog.CreateCarrier(new Carrier
                {
                    Name = model.Name,
                    Status = PartnerStatus.Disabled,
                    Description = model.Description,
                    Settings = new Dictionary<string, string>()
                });

                if (model.IsSettingsRequired())
                {
                    var carrier = _catalog.GetCarrierById(id);
                    if (carrier == null)
                        return RedirectToAction("Index", "Carriers");

                    var editModel = CarrierEditModel.Map(carrier);
                    editModel.EditMode = PartnerEditMode.Setup;
                    editModel.Status = model.Status;
                    editModel.MapSettings(carrier.Settings);

                    return View("Edit", editModel);
                }

                return RedirectToAction("Index", "Carriers");
            }
            catch (PartnerNameAlreadyExistsException)
            {
                ModelState.AddModelError("Name", string.Format("Партнер с наименованием \"{0}\" уже существует", model.Name));

                return View("Edit", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var carrier = _catalog.GetCarrierById(id);
            if (carrier == null)
                return HttpNotFound();

            var model = CarrierEditModel.Map(carrier);
            model.MapSettings(carrier.Settings);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarrierEditModel model)
        {
            var carrier = _catalog.GetCarrierById(model.CarrierId);
            if (carrier == null)
                return HttpNotFound();

            model.ValidateSettings(carrier.Settings, ModelState);

            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            try
            {
                _catalog.UpdateCarrier(new Carrier
                {
                    Id = model.CarrierId,
                    Name = model.Name,
                    Status = model.Status.Map(),
                    Description = model.Description,
                    Settings = model.Settings.MergeSettings(carrier.Settings)
                });

                return RedirectToAction("Index", "Carriers");
            }
            catch (PartnerNameAlreadyExistsException)
            {
                ModelState.AddModelError("Name", string.Format("Партнер с наименованием \"{0}\" уже существует", model.Name));

                return View("Edit", model);
            }
        }
    }
}
