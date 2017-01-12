using System;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Otp;
using Vtb24.Site.Security;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;

namespace Vtb24.Site.Controllers
{
    public class OtpController : BaseController
    {
        public OtpController(IOneTimePasswordService otp)
        {
            _otp = otp;
        }

        private readonly IOneTimePasswordService _otp;

        public ActionResult Confirm(string token, string otp)
        {
            try
            {
                var result = _otp.Confirm(
                    new ConfirmOtpParameters
                    {
                        OtpToken = token,
                        Otp = otp
                    }
                );

                return Json(
                    new ConfirmationResultModel
                    {
                        Confirmed = result,
                        Status = ConfirmationResultModel.Statuses.Success
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (OtpExpiredException)
            {
                return Json(
                    new ConfirmationResultModel
                    {
                        Confirmed = false,
                        Status = ConfirmationResultModel.Statuses.TokenExpired
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (OtpAllowedAttemptsExceeded)
            {
                return Json(
                    new ConfirmationResultModel
                    {
                        Confirmed = false,
                        Status = ConfirmationResultModel.Statuses.AttemptsExceeded
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        public ActionResult Renew(string token)
        {
            try
            {

                var result = _otp.Renew(
                    new RenewOtpParameters
                    {
                        OtpToken = token
                    }
                );

                return Json(
                    RenewResultModel.Map(result),
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (InvalidOtpTokenException)
            {
                return Json(
                    new RenewResultModel { Status = RenewResultModel.Statuses.TokenNotFound },
                    JsonRequestBehavior.AllowGet
                );
            }
        }


        public static bool HandleOtpException(OneTimePasswordServiceException e, ModelStateDictionary state, IOtpModel model)
        {
            if (e is OtpExpiredException)
            {
                state.AddModelError(
                    "Otp",
                    "Время действия кода подтверждения истекло. Воспользуйтесь ссылкой \"Выслать код подтвердения повторно\""
                );
                model.ExpirationTimeUtc = DateTime.UtcNow;
                
                return true;
            }

            if (e is OtpAllowedAttemptsExceeded)
            {
                state.AddModelError(
                    "Otp",
                    "Превышено количество попыток ввода кода подтверждения. Воспользуйтесь ссылкой \"Выслать код подтвердения повторно\""
                );
                model.ExpirationTimeUtc = DateTime.UtcNow;
                return true;
            }

            return false;
        }
    }
}