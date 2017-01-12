using System.Web.Mvc;

namespace Vtb24.Arms.AdminSecurity
{
    public class AdminSecurityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminSecurity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdminSecurity_default",
                "AdminSecurity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
