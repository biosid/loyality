using System.Web.Optimization;

namespace Vtb24.Site.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/html5shiv")
                .Include("~/Scripts/html5shiv.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/global")
                .Include("~/Scripts/require.js", "~/Scripts/App/require-config.js")
                .Include("~/Scripts/modernizr-{version}.js")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.unobtrusive*","~/Scripts/jquery.validate*")
                .Include("~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery.ui.datepicker-ru.js")
                .Include("~/Scripts/jquery.globalize/globalize.js", "~/Scripts/jquery.globalize/cultures/globalize.culture.ru-RU.js", "~/Scripts/App/jquery.globalize-config.js")
                .Include("~/Scripts/jquery.exactcomplete.js")
                .Include("~/Scripts/jquery.maskedinput-1.3.1.js")
                .Include("~/Scripts/jquery.tinycarousel.js")
                .Include("~/Scripts/jquery.alphanum.customized.js")
                .Include("~/Scripts/jquery.ba-outside-events.js")
                .Include("~/Scripts/jquery.scrollTo.js")
                .Include("~/Scripts/magnific-popup.custom.js")
                .Include("~/Scripts/jquery.placeholder.js")
                .Include("~/Scripts/jquery.mousewheel.js")
                .Include("~/Scripts/select2.js")
                .Include("~/Scripts/select2_locale_ru.js")
                .Include("~/Scripts/legacy/carousel.js")
                .Include("~/Scripts/legacy/johndyer-mediaelement/build/mediaelement-and-player.js")
                .Include("~/Scripts/jquery.ba-throttle-debounce.js")
                .Include("~/Scripts/jquery.menu-aim.js")
                .IncludeDirectory("~/Scripts/App/", "*.js", true)
            );

            bundles.Add(new StyleBundle("~/Content/style")
                .Include("~/Content/css/select2.css", "~/Content/css/select2.custom.css")
                .Include("~/Content/magnific-popup.css")
                .Include("~/Scripts/legacy/johndyer-mediaelement/build/mediaelementplayer.css")   
                //.Include("~/Scripts/legacy/flowplayer/skin/minimalist.css")
                .Include("~/Content/style.css")
                .Include("~/Content/inbox.css")
            );


            bundles.Add(new StyleBundle("~/Content/ie")
                .Include("~/Content/ie.css")
            );

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.css",
                        "~/Content/themes/base/jquery.ui.core.css",
                        //"~/Content/themes/base/jquery.ui.resizable.css",
                        //"~/Content/themes/base/jquery.ui.selectable.css",
                        //"~/Content/themes/base/jquery.ui.accordion.css",
                        //"~/Content/themes/base/jquery.ui.autocomplete.css",
                        //"~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        //"~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css")
            );
        }
    }
}