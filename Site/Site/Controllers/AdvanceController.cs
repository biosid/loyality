using System;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Advance;
using Vtb24.Site.Models.Buy;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services;

namespace Vtb24.Site.Controllers
{
    public class AdvanceController : BaseController
    {
        public AdvanceController(ClientPrincipal principal, IBuy buy)
        {
            _principal = principal;
            _buy = buy;
        }

        private readonly ClientPrincipal _principal;
        private readonly IBuy _buy;

        // отображается в iframe
        [HttpGet]
        public ActionResult Pay(PayAdvanceModel model, bool? refreshDisabled)
        {
            if (!_principal.IsAuthenticated)
            {
                return View("PostCommand",
                            refreshDisabled.HasValue && refreshDisabled.Value
                                ? PostCommandModel.ShowBuyFailed()
                                : PostCommandModel.ReloadPayAdvanceFrame(model));
            }

            try
            {
                var baseUrl = System.Configuration.ConfigurationManager.AppSettings["pay_advance_return_url_base"]
                              ?? string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

                var returnUrlSuccess = baseUrl + Url.Action("Success", "Advance", model);
                var returnUrlFail = baseUrl + Url.Action("Fail", "Advance");

                var parameters = _buy.GetAdvancePaymentFormParameters(model.OrderId, model.OtpToken, returnUrlSuccess, returnUrlFail);

                var formModel = new PaymentFormModel
                {
                    Url = parameters.Url,
                    Method = parameters.Method,
                    Parameters = parameters.Parameters
                };

                return View("Pay", formModel);
            }
            catch (Exception e)
            {
                LogError(e);
                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }
        }

        // отображается в iframe
        [HttpGet]
        public ActionResult Success(PayAdvanceModel model)
        {
            return View("PostCommand", PostCommandModel.ShowBuyConfirm(new ConfirmOrderModel
            {
                OrderDraftId = model.OrderId,
                OtpToken = model.OtpToken
            }));
        }

        // отображается в iframe
        [HttpGet]
        public ActionResult Fail()
        {
            return View("PostCommand", PostCommandModel.ShowBuyFailed());
        }
    }
}
