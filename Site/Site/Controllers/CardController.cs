using System;
using System.Text;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Card;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services;

namespace Vtb24.Site.Controllers
{
    public class CardController : BaseController
    {
        public CardController(ICardRegistration card, ClientPrincipal principal)
        {
            _card = card;
            _principal = principal;
        }

        private readonly ICardRegistration _card;
        private readonly ClientPrincipal _principal;

        // отображается в iframe
        [HttpGet]
        public ActionResult Register(string returnUrl, bool? refreshDisabled)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                LogError("Значение параметра returnUrl неверно");
                return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
            }

            if (!_principal.IsAuthenticated)
            {
                return View("PostCommand",
                            refreshDisabled.HasValue && refreshDisabled.Value
                                ? PostCommandModel.ShowCardRegistrationError()
                                : PostCommandModel.ReloadCardRegisterFrame(returnUrl));
            }

            try
            {
                var parameters = _card.GetRegistrationParameters();

                var returnUrlBytes = Encoding.UTF8.GetBytes(returnUrl);
                var returnUrlBase64 = Convert.ToBase64String(returnUrlBytes);

                var model = CardRegistrationModel.Map(parameters);
                model.SuccessUrl = Url.Action("Success", "Card", new { returnUrlBase64 });
                model.ErrorUrl = Url.Action("ShowError", "Card");

                return View("Register", model);
            }
            catch (Exception e)
            {
                LogError(e);
                return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
            }
        }

        // отображается в iframe
        [HttpGet]
        public ActionResult Success(string returnUrlBase64, bool? refreshDisabled)
        {
            if (!_principal.IsAuthenticated)
            {
                return View("PostCommand",
                            refreshDisabled.HasValue && refreshDisabled.Value
                                ? PostCommandModel.ShowCardRegistrationError()
                                : PostCommandModel.ReloadCardSuccessFrame(returnUrlBase64));
            }

            if (string.IsNullOrEmpty(returnUrlBase64))
            {
                LogError("отсутствует значение параметра returnUrlBase64");
                return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
            }

            var returnUrlBytes = Convert.FromBase64String(returnUrlBase64);
            var returnUrl = Encoding.UTF8.GetString(returnUrlBytes);

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                LogError("Значение параметра returnUrl неверно");
                return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
            }

            try
            {
                // TODO: необходимо доработать данную функцию на бэке, чтобы происходила проверка статуса пробного списания средств с карты
                _card.RegisterCard();
                return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                LogError(e);
                return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
            }
        }

        // отображается в iframe
        [HttpGet]
        public ActionResult ShowError()
        {
            return View("PostCommand", PostCommandModel.ShowCardRegistrationError());
        }

        // отображается в iframe
        [HttpGet]
        public ActionResult ReloadParent()
        {
            return View("PostCommand", PostCommandModel.Refresh());
        }

        [HttpGet]
        public ActionResult RegistrationError()
        {
            return View("RegistrationError");
        }
    }
}
