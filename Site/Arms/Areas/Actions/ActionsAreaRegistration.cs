using System.Web.Mvc;
using Vtb24.Arms.Actions.Models;

namespace Vtb24.Arms.Actions
{
    public class ActionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Actions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Actions_controller",
                "Actions/{mode}/{action}/{id}",
                new { controller = "Actions", mode = ActionsOperationMode.Actual, action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Actions_default",
                "Actions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
