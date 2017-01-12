using System.Drawing.Imaging;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;

namespace Vtb24.Site.Infrastructure
{
    public class CaptchaResult : ActionResult
    {       
        private readonly CaptchaOptions _options;
        public CaptchaResult(CaptchaOptions options)
        {
            _options = options;
        }        

        public override void ExecuteResult(ControllerContext context)
        {            
            _options.Text = Captcha.GenerateRandomCode();                       
            var c = new Captcha(_options);
            HttpContext.Current.Cache["captcha_" + c.Id] = _options.Text;
            var myCookie = new HttpCookie("CurrentCaptcha");
            myCookie["CurrentCaptchaId"] = c.Id.ToString();
            myCookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(myCookie);

            var cb = context.HttpContext;
            cb.Response.Clear();
            cb.Response.ContentType = "image/jpeg";

            c.Image.Save(cb.Response.OutputStream, ImageFormat.Jpeg);
            c.Image.Dispose();
            cb.Response.End();
        }
    }
}