using System;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Vtb24.Site.Content.Advertisements;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Helpers;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Account;
using Vtb24.Site.Models.Account.Formatters;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Security;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.Services;
using Vtb24.Site.Services.ClientService;
using Vtb24.Site.Services.ClientTargeting;
using Vtb24.Site.Services.Profile;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Inputs;
using Vtb24.Site.Services.VtbBankConnector;
using Vtb24.Site.Services.VtbBankConnector.Models;
using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;
using Vtb24.Site.Services.VtbBankConnector.Models.Exceptions;

namespace Vtb24.Site.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ISecurityService security, IRegistration reg, IOneTimePasswordService otp, IProfile profile, IClientTargeting clientTargeting, IVtbBankConnectorService bankConnector, CookieSessionStorage cookie, IAdvertisementsManagement advertisementsManagement)
        {
            _security = security;
            _reg = reg;
            _otp = otp;
            _profile = profile;
            _clientTargeting = clientTargeting;
            _bankConnector = bankConnector;
            _cookie = cookie;
            _advertisementsManagement = advertisementsManagement;
        }

        private readonly ISecurityService _security;
        private readonly IRegistration _reg;
        private readonly IOneTimePasswordService _otp;
        private readonly IProfile _profile;
        private readonly IClientTargeting _clientTargeting;
        private readonly IVtbBankConnectorService _bankConnector;
        private readonly CookieSessionStorage _cookie;
        private readonly IAdvertisementsManagement _advertisementsManagement;

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("admin"))
            {
                return RedirectToLocal(returnUrl);
            }

            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View("Login", model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var login = LoginFormatter.FormatPhone(model.Phone);

                try
                {
                    if (Login(login, model.Password))
                    {
                        return RedirectToLocal(model.ReturnUrl);
                    }
                    ModelState.AddModelError("", "Логин или пароль неверен, попробуйте ввести ещё раз");

                    LogInfo(
                        "Неверный логин или пароль для входа на сайт",
                        new { login, password = model.Password });
                }
                // сценарий с превышеным кол-вом ввода пароля
                catch (UserLockedOutException e)
                {
                    LogInfo(e, new { login });

                    var message = string.Format(
                        "Превышено допустимое количество попыток ввода пароля. Вы можете воспользоваться ссылкой «Я забыл свой пароль» или повторить через {0}",
                        e.IntervalInMinutes.Pluralize("{1} минуту", "{2} минуты", "{5} минут")
                    );
                    ModelState.AddModelError("", message);
                }
                // сценарий с заблокированным пользователем
                catch (UserDisabledException e)
                {
                    LogInfo(e, new { login });

                    ModelState.AddModelError("", "Пользователь заблокирован");
                }
                // сценарий с обязательной сменой пароля
                catch (PasswordNotSetException e)
                {
                    LogInfo(e, new { login });

                    var pwdModel = new CreatePasswordModel
                    {
                        Phone = model.Phone,
                        Password = model.Password,
                        ReturnUrl = model.ReturnUrl
                    };
                    return View("CreatePassword", pwdModel);
                }
            }

            return View("Login", model);
        }

        [HttpPost]
        public ActionResult CreatePassword(CreatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var login = LoginFormatter.FormatPhone(model.Phone);

                var options = new ChangePasswordOptions
                {
                    Login = login,
                    Password = model.Password,
                    NewPassword = model.NewPassword
                };
                var result = _security.ChangePassword(options);

                if (!result)
                {
                    throw new SecurityException(string.Format(
                        "Неверный временный пароль или номер телефона ({0}) при создании нового пароля."
                        + " Вероятно, клиент уже создал постоянный пароль, либо поменял телефон.",
                        model.Phone
                    ));
                }

                Login(login, model.NewPassword);

                return RedirectToLocal(model.ReturnUrl);
            }

            return View("CreatePassword", model);
        }

        [HttpGet, Authorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            return View("ChangePassword", model);
        }

        [HttpPost, Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var login = User.Identity.Name;
                var options = new ChangePasswordOptions
                {
                    Login = login,
                    Password = model.Password,
                    NewPassword = model.NewPassword
                };
                var result = _security.ChangePassword(options);

                if (result)
                {
                    return View("PasswordChanged");
                }

                LogInfo("Неверный пароль при смене пароля", new { login, password = model.Password });

                ModelState.AddModelError("", "Неверный пароль, убедитесь в правильности ввода данных");
            }

            return View("ChangePassword", model);
        }

        [HttpGet]
        public ActionResult ForgotPassword(string phone)
        {
            var model = new ForgotPasswordModel { Phone = phone };
            return View("ForgotPassword", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", model);
            }

            var phone = SanitizePhoneNumber(model.Phone);

            var profile = GetProfileByPhoneNumber(phone);
            var email = profile != null ? profile.Email : null;
            var hasEmail = !string.IsNullOrWhiteSpace(email);
            var isVip = profile != null && IsVipClient(profile.ClientId);

            try
            {
                var sendOptions = new SendChangePasswordOtpOptions
                {
                    Login = phone,
                    Email = email,
                    ForceSms = isVip,
                    SmsOtpMessageTemplate = GetResetPasswordSmsMessageTemplate(),
                    EmailOtpMessageTemplate = GetResetPasswordEmailMessageTemplate()
                };
                var otp = _security.SendChangePasswordOtp(sendOptions);

                var resetModel = new ResetPasswordModel
                {
                    Phone = model.Phone,
                    OtpToken = otp.OtpToken,
                    ExpirationTimeUtc = otp.ExpirationTimeUtc,
                    IsEmailRequired = !isVip && !hasEmail,
                    Hint = GetResetPasswordHint(otp.DeliveryMeans)
                };

                return View("ResetPassword", resetModel);
            }
            catch (TooFrequentOtpSendException ex)
            {
                LogInfo(ex, new { login = phone });

                ModelState.AddModelError("", ex.Format("сбросить пароль"));
            }
            catch (TooManyOtpSendAttemptsException ex)
            {
                LogInfo(ex, new { login = phone });

                ModelState.AddModelError("", ex.Format("сбросить пароль"));
            }
            catch (UserDisabledException ex)
            {
                LogInfo(ex, new { login = phone });

                ModelState.AddModelError("", "Пользователь заблокирован");
            }
            catch (OtpViaSmsLimitReached ex)
            {
                LogInfo(ex, new { login = phone });

                return View("PasswordResetUnavailable");
            }

            return View("ForgotPassword", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var phone = SanitizePhoneNumber(model.Phone);

            var deliveryMeans = _otp.GetDeliveryMeans(new GetDeliveryMeansParameters { OtpToken = model.OtpToken });

            model.Hint = GetResetPasswordHint(deliveryMeans ?? OtpDeliveryMeans.Sms);

            if (!model.IsEmailRequired)
            {
                ModelState.RemoveErrors(k => k == "Email" || k == "ConfirmEmail");
            }

            if (!ModelState.IsValid)
            {
                return View("ResetPassword", model);
            }

            try
            {
                var optParams = new ConfirmOtpParameters
                {
                    OtpToken = model.OtpToken,
                    Otp = model.Otp
                };

                if (!_otp.Confirm(optParams))
                {
                    LogInfo(
                        "Неверный код подтверждения при вводе пароля",
                        new { login = phone, tokenOtp = model.OtpToken, otp = model.Otp });

                    ModelState.AddModelError("Otp", "Неверный код подтверждения");
                    return View("ResetPassword", model);
                }
            }
            catch (OneTimePasswordServiceException e)
            {
                if (OtpController.HandleOtpException(e, ModelState, model))
                {
                    LogInfo(e, new { login = phone });
                    return View("ResetPassword", model);
                }
                throw;
            }

            var changeOptions = new ChangePasswordWithOtpOptions
            {
                Login = phone,
                NewPassword = model.NewPassword,
                OtpToken = model.OtpToken
            };
            _security.ChangePasswordWithOtp(changeOptions);

            if (model.IsEmailRequired)
            {
                try
                {
                    var user = _security.GetUserByPhoneNumber(phone);
                    if (user == null)
                    {
                        LogError("не найден пользователь при обновлении Email после восстановления папроля");
                    }
                    else
                    {
                        _bankConnector.UpdateClientEmail(user.ClientId, model.Email);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }

            Login(phone, model.NewPassword);

            return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _security.Logout();
            return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Registration", new RegistrationModel());
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {
            if (model.BirthYear.HasValue && model.BirthYear < DateTime.Now.Year - 100 || model.BirthYear > DateTime.Now.Year - 18)
            {
                ModelState.AddModelError("", "Неверный год, убедитесь в правильности ввода данных");
            }
            else if (model.BirthYear.HasValue && model.BirthMonth.HasValue && model.BirthDate.HasValue)
            {
                if (model.BirthDate < 1 || model.BirthDate > DateTime.DaysInMonth(model.BirthYear.Value, model.BirthMonth.Value))
                {
                    ModelState.AddModelError("", "Укажите корректную дату рождения");
                }
            }

            if (!model.AgreeToTerms)
            {
                ModelState.AddModelError("", "Вы должны согласиться с условиями лицензионного соглашения и дать согласие на обработку своих персональных данных");
            }

            if (ModelState.IsValid)
            {
                // уберем лишние пробелы
                model.FirstName = Regex.Replace(model.FirstName.Trim(), "\\s+", " ");
                model.LastName = Regex.Replace(model.LastName.Trim(), "\\s+", " ");
                if (model.MiddleName != null)
                    model.MiddleName = Regex.Replace(model.MiddleName.Trim(), "\\s+", " ");

                try
                {
                    _reg.Register(
                        new RegisterClientParams
                            {
                                Email = model.Email,
                                FirstName = model.FirstName,
                                Gender = Gender.Other,
                                LastName = model.LastName,
                                MiddleName = model.MiddleName,
                                Phone = SanitizePhoneNumber(model.Phone),
                                BirthDate = new DateTime(model.BirthYear.Value, model.BirthMonth.Value, model.BirthDate.Value)
                            });

                    return RedirectToAction("Registered", "Account");
                }
                catch (VtbBankConnectorClientAlreadyExistsException e)
                {
                    LogInfo(e, new { phone = model.Phone });
                    ModelState.AddModelError("", "Такой номер телефона уже зарегистрирован, регистрация невозможна");
                }
            }

            return View("Registration", model);
        }

        public ActionResult Registered()
        {
            return View("ThankYou");
        }

        private bool Login(string login, string password)
        {
            if (!_security.Login(login, password))
            {
                return false;
            }

            try
            {
                // находим пользователя сайта
                var user = _security.GetUserByPhoneNumber(login);
                if (user != null)
                {
                    // обновляем текущее местоположение в профиле (из location cookie)
                    UpdateProfileLocation(user.ClientId);

                    // настраиваем отображение целевой рекламы
                    PrepareAdvertisement(user.ClientId);

                    // проверями необходимость отобразить предложение ввести свой Email
                    CheckEmailInProfile(login);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, new { login });
            }

            return true;
        }

        private void PrepareAdvertisement(string clientId)
        {
            var advertisementToShow = _advertisementsManagement
                .GetAdvertisements(clientId, PagingSettings.ByOffset(0, 1))
                .FirstOrDefault();

            if (advertisementToShow == null)
            {
                return;
            }

            if (advertisementToShow.Advertisement.ShowUntil.HasValue &&
                DateTime.Now >= advertisementToShow.Advertisement.ShowUntil.Value)
            {
                return;
            }

            if (advertisementToShow.Advertisement.MaxDisplayCount.HasValue &&
                advertisementToShow.ShowCounter >= advertisementToShow.Advertisement.MaxDisplayCount.Value)
            {
                return;
            }

            var model = AdvertisementModel.Map(advertisementToShow);
            TempData.SetActiveAdvertisement(model);

            _advertisementsManagement.IncreaseShowCounter(advertisementToShow.Advertisement_Id, clientId);
        }

        private void UpdateProfileLocation(string clientId)
        {
            // получаем местоположение из куки (оно ставится в Global.asax)
            var location = _cookie.GetUserLocation();

            // сохраняем местоположение в профиль
            _profile.SetLocation(new SetLocationParameters
            {
                ClientId = clientId,
                LocationKladr = location.KladrCode,
                LocationTitle = location.Title
            });
        }

        private void CheckEmailInProfile(string phone)
        {
            if (!_security.OtpViaEmailEnabled)
            {
                return;
            }

            var profile = GetProfileByPhoneNumber(phone);

            if (profile != null && string.IsNullOrWhiteSpace(profile.Email))
            {
                TempData.ProposeToSetEmail();
            }
        }

        private ClientProfile GetProfileByPhoneNumber(string phone)
        {
            var user = _security.GetUserByPhoneNumber(phone);

            var profile = user != null
                              ? _profile.GetProfile(new GetProfileParameters { ClientId = user.ClientId })
                              : null;

            return profile;
        }

        private bool IsVipClient(string clientId)
        {
            var groups = _clientTargeting.GetClientGroups(clientId);

            return groups != null && groups.Any(g => g.IsSegment && g.Id == "VIP");
        }

        private static string SanitizePhoneNumber(string phone)
        {
            return Regex.Replace(phone, "[^\\d]", string.Empty);
        }

        private static string GetResetPasswordHint(OtpDeliveryMeans deliveryMeans)
        {
            switch (deliveryMeans)
            {
                case OtpDeliveryMeans.Email:
                    return Constants.PASSWORD_RESET_VIA_EMAIL_HINT;
                case OtpDeliveryMeans.Sms:
                    return Constants.PASSWORD_RESET_VIA_SMS_HINT;

                default:
                    throw new NotSupportedException();
            }
        }

        private static string GetResetPasswordSmsMessageTemplate()
        {
            var template = ConfigurationManager.AppSettings["security_change_pwd_otp_sms_notification"];

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ConfigurationErrorsException(
                    "Не задан шаблон СМС-сообщения для напоминания пароля (appSettings/security_change_pwd_otp_sms_notification)");
            }

            return template;
        }

        private static string GetResetPasswordEmailMessageTemplate()
        {
            var template = ConfigurationManager.AppSettings["security_change_pwd_otp_email_notification"];

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ConfigurationErrorsException(
                    "Не задан шаблон Email-сообщения для напоминания пароля (appSettings/security_change_pwd_otp_email_notification)");
            }

            return template;
        }
    }
}
