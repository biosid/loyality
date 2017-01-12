using System.Web.Mvc;

namespace Vtb24.Site.Helpers
{
    public static class CaptchaHelper
    {
        public static string CaptchaUrl(this UrlHelper helper, int width, int height)
        {
            return helper.Action("RenderCaptcha", "Captcha", new {width, height});
        }
    }
}