using System.Web.Optimization;

namespace Rapidsoft.VTB24.Reports.Site.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/scripts")
                    .Include("~/Scripts/jquery-1.9.1.js")
                    .Include("~/Scripts/bootstrap.js")
                    .Include("~/Scripts/bootstrap-datepicker.js")
                    .Include("~/Scripts/bootstrap-datepicker.ru.js")
                    .Include("~/Scripts/jquery.validate.js")
                    .Include("~/Scripts/App/jquery.validate.config.js")
                    .Include("~/Scripts/jquery.validate.unobtrusive.js")
                );

            bundles.Add(
                new StyleBundle("~/bundles/styles")
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/bootstrap-theme.css")
                    .Include("~/Content/datepicker3.css")
                );
        }
    }
}
