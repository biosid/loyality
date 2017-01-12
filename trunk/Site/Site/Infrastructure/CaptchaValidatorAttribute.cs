using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Vtb24.Site.Infrastructure
{   
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]    
    public class CaptchaValidatorAttribute : ValidationAttribute
    {                   
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (HttpContext.Current.Request.Cookies["CurrentCaptcha"] == null)
            {
                return false;
            }

            var str = value as string;

            var captchaId = HttpContext.Current.Request.Cookies["CurrentCaptcha"]["CurrentCaptchaId"];

            if (string.IsNullOrEmpty(captchaId))
            {
                return false;
            }

            var captcha = HttpContext.Current.Cache["captcha_" + captchaId];

            return captcha != null && captcha.ToString() == str;
        }
    }
}