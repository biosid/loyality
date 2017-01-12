using System;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Vtb24.ServicesExtensions.ServiceLogger;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.SmsBallance;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.SmsBallance;

namespace Vtb24.Site.Controllers
{
    public class SmsBallanceController : BaseController
    {
        public SmsBallanceController(ISmsBallanceService smsBallance)
        {
            _smsBallance = smsBallance;
        }

        private readonly ISmsBallanceService _smsBallance;

        [HttpGet]
        public ActionResult Index(SmsBallanceModel model)
        {
            try
            {
                var requestMessageId = model.messageId ?? model.incomingId; // по спецификации должно приходить messageId, но приходит incomingId
                
                var checkString = string.Format("{0}{1}{2}", model.clientId, model.message, requestMessageId);

                if (!_smsBallance.ValidateRequestHash(checkString, model.hash))
                {
                    throw new SecurityException(string.Format("Неверная подпись запроса при вызове сервиса получения баланса по СМС ({0})", Request.RawUrl));
                }

                var phoneNumber = Regex.Replace(model.clientId, "[^\\d]", string.Empty);
                var ballance = _smsBallance.GetAccountBallance(phoneNumber);

                var message = AppSettingsHelper.String("sms_ballance_message", "Ваш баланс: {0}");
                return Content(string.Format(new PluralizerFormatter(), message, ballance));
            }
            catch (UserNotFoundException)
            {
                var message = AppSettingsHelper.String("sms_ballance_error_message", "Ваш номер не зарегистрирован в программе Коллекция");
                return Content(message);
            }
        }

    }
}
