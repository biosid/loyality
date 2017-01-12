using System.Web.Mvc;
using ImageResizer;
using ImageResizer.Plugins.RemoteReader;

namespace Vtb24.Site.Helpers
{
    public static class UrlHelpers
    {
        public static string ImageProcessor(this UrlHelper html, string profile, string src)
        {
            if (string.IsNullOrEmpty(profile) || string.IsNullOrEmpty(src))
            {
                return src;
            }

            var section = (ResizerSection)System.Configuration.ConfigurationManager.GetSection("resizer");
            if (section == null || section.getAttr("working.on", "false") == "false")
            {
                return src;
            }
            
            return RemoteReaderPlugin.Current.CreateSignedUrl(src, string.Format("preset={0}", profile));
        }

        public static string Image(this UrlHelper html, string profile, string src)
        {
            if (string.IsNullOrEmpty(profile) || string.IsNullOrEmpty(src))
            {
                return src;
            }

            var section = (ResizerSection)System.Configuration.ConfigurationManager.GetSection("resizer");
            if (section == null || section.getAttr("working.on", "false") == "false")
            {
                return src;
            }

            return string.Format("{0}?preset={1}", src, html.Encode(profile));
        }
    }
}