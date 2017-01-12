using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Controllers
{
    public class CaptchaController : BaseController
    {
        [NoCacheHeader]
        public ActionResult RenderCaptcha(int width, int height)
        {
            return new CaptchaResult(new CaptchaOptions() { Width = width, Height = height, FontSize = 14 });
        }
    }
}
