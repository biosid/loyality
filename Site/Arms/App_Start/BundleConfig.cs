using System.Web.Optimization;

namespace Vtb24.Arms.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/style")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/styles.css")
                .Include("~/Content/jquery.tagsinput.css")
                .Include("~/Content/bootstrap-fileupload.css")
                .Include("~/Content/armadmin.css")
                .Include("~/Content/filterbuilder.css")
                .Include("~/Content/filterbuilder-tree.css")
                .Include("~/Content/jquery.tagsinput.css")
                .Include("~/Content/Codemirror/codemirror.css")
                .Include("~/Content/Codemirror/xq-light.css")
                .Include("~/Content/ElFinder/css/elfinder.full.css")
                .Include("~/Content/ElFinder/css/theme.css")
                .Include("~/Content/css/select2.css", "~/Content/css/select2.custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/global")
                .Include("~/Scripts/require.js", "~/Areas/Catalog/Scripts/App/require-config.js")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*")
                .Include("~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery.ui.datepicker-ru.js")
                .Include("~/Scripts/jquery.globalize/globalize.js", "~/Scripts/jquery.globalize/cultures/globalize.culture.ru-RU.js", "~/Scripts/App/jquery.globalize-config.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/jquery.sticky.js") // TODO: заменить
                .Include("~/Scripts/jquery.tagsinput.js")
                .Include("~/Scripts/jquery.tree.js")
                .Include("~/Scripts/jquery.ba-outside-events.js")
                .Include("~/Scripts/jquery.alphanum.customized.js")
                .Include("~/Scripts/jquery.maskedinput-1.3.1.js")
                .Include("~/Scripts/jquery.placeholder.js")
                .Include("~/Scripts/bootstrap-fileupload.js")
                .Include("~/Scripts/select2.js")
                .Include("~/Scripts/select2_locale_ru.js")
                .Include("~/Scripts/Codemirror/lib/codemirror.js")
                .Include("~/Scripts/Codemirror/addon/edit/closebrackets.js")
                .Include("~/Scripts/Codemirror/addon/edit/closetag.js")
                .Include("~/Scripts/Codemirror/mode/xml/xml.js")
                .Include("~/Scripts/Codemirror/mode/css/css.js")
                .Include("~/Scripts/Codemirror/mode/vbscript/vbscript.js")
                .Include("~/Scripts/Codemirror/mode/htmlmixed/htmlmixed.js")
                .Include("~/Scripts/Codemirror/mode/javascript/javascript.js")
                .Include("~/Scripts/jquery-migrate-{version}.js")
                .Include("~/Scripts/ElFinder/elfinder.full.js")
                .Include("~/Scripts/ElFinder/i18n/elfinder.ru.js")
                .Include("~/Scripts/jquery.dotdotdot.js")
                .Include("~/Scripts/to-markdown.js")
                .Include("~/Scripts/marked.js")
                .IncludeDirectory("~/Scripts/App/", "*.js", true)
            );

            bundles.Add(new StyleBundle("~/content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}