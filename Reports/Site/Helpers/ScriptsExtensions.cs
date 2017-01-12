using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Rapidsoft.VTB24.Reports.Site.Helpers
{
    public static class ScriptsExtensions
    {
        public static IHtmlString AddScriptBlock(this HtmlHelper helper, Func<object, HelperResult> template)
        {
            Scripts(helper.ViewContext.HttpContext).Add(template(null));
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper helper)
        {
            var scripts = Scripts(helper.ViewContext.HttpContext);

            foreach (var block in scripts)
            {
                helper.ViewContext.Writer.Write(block);
            }

            return MvcHtmlString.Empty;
        }

        private static List<HelperResult> Scripts(HttpContextBase context)
        {
            const string name = "__scripts_extensions__";

            if (!context.Items.Contains(name))
            {
                context.Items.Add(name, new List<HelperResult>());
            }

            return context.Items[name] as List<HelperResult>;
        }
    }
}
