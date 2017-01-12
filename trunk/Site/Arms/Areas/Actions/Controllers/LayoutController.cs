using System;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.Actions.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Actions.Controllers
{
    public class LayoutController : BaseController
    {
        [ChildActionOnly]
        public ActionResult Tabs(ActionsOperationMode operationMode)
        {
            var model = Enum.GetValues(typeof (ActionsOperationMode))
                .Cast<ActionsOperationMode>()
                .Select(mode => new TabModel
                {
                    Title = mode.GetTitle(),
                    Url = Url.Action("Index", "Actions", new { mode }),
                    IsActive = mode == operationMode
                })
                .ToArray();

            return View("_Tabs", model);
        }
    }
}
