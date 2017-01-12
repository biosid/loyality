using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Security.Models.CustomFields;

namespace Vtb24.Arms.Security.Controllers
{
    public class CustomFieldsController : BaseController
    {
        public CustomFieldsController(IAdminVtbBankConnector bankConnector)
        {
            _bankConnector = bankConnector;
        }

        private readonly IAdminVtbBankConnector _bankConnector;

        [HttpGet]
        public ActionResult Index()
        {
            var customFields = _bankConnector.GetAllCustomFields();

            var model = customFields.Select(CustomFieldModel.Map).ToArray();

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Append(AppendCustomFieldModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            try
            {
                var name = model.Name.Trim();
                var id = _bankConnector.AppendCustomField(name);
                return JsonSuccess(new { id, name });
            }
            catch (AdminVtbBankConnectorCustomFieldAlreadyExists)
            {
                ModelState.AddModelError("Name", "Дополнительное поле с указанным наименованием уже существует");
                return JsonErrors();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id)
        {
            _bankConnector.RemoveCustomField(id);
            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(RenameCustomFieldModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            try
            {
                _bankConnector.RenameCustomField(model.Id, model.Name.Trim());
                return JsonSuccess();
            }
            catch (AdminVtbBankConnectorCustomFieldAlreadyExists)
            {
                ModelState.AddModelError("Name", "Дополнительное поле с указанным наименованием уже существует");
                return JsonErrors();
            }
        }
    }
}
