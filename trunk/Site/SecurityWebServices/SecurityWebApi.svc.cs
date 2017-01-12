using System.Collections.Generic;
using System.Configuration;
using Vtb24.Common.Configuration;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.SecurityWebServices.Security;
using Vtb24.Site.SecurityWebServices.Security.Models;
using Vtb24.Site.SecurityWebServices.Security.Models.Inputs;
using Vtb24.Site.SecurityWebServices.Security.Models.Outputs;
using Vtb24.Site.Services.SmsService.Models.Exceptions;

namespace Vtb24.Site.SecurityWebServices
{
    public class SecurityWebApi : ISecurityWebApi
    {
        public SecurityWebApi(ISecurityService security)
        {
            _security = security;
        }

        private readonly ISecurityService _security;

        public void CreateUser(CreateUserOptions options)
        {
            var messageKey = options.RegistrationType == RegistrationType.SiteRegistration
                          ? "security_create_user_notification__site"
                          : "security_create_user_notification__bank";

            var message = ConfigurationManager.AppSettings[messageKey];

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ConfigurationErrorsException(
                    string.Format("Не задан шаблон СМС сообшения о регистрации (appSettings/{0})", messageKey)
                );
            }

            var smsType = options.RegistrationType == RegistrationType.SiteRegistration
                              ? BankSmsType.LoyaltyRegistrationSucceeded
                              : BankSmsType.BankRegistrationSucceeded;

            var parameters = new CreateClientAccountOptions
            {
                ClientId = options.ClientId,
                PhoneNumber = options.PhoneNumber,
                WelcomeMessageTemplate = message,
                WelcomeSmsType = smsType
            };
            _security.CreateUser(parameters);
        }

        public void DenyRegistrationRequest(DenyRegistrationRequestOptions options)
        {
            options.ValidateAndThrow();

            string messageKey;
            BankSmsType smsType;

            switch (options.RegistrationRequestBankStatus)
            {
                // Клиент уже был зарегистрирован ранее
                case 2:
                    messageKey = "security_deny_already_registered_client_notification";
                    smsType = BankSmsType.RegistrationDeniedAlreadyRegistered;
                    break;
                // Клиент известен, но не может быть подключён к программе
                case 4:
                    messageKey = "security_deny_client_notification";
                    smsType = BankSmsType.RegistrationDeniedNoCards;
                    break;
                // Неизвестный клиент
                case 5:
                    messageKey = "security_deny_unknown_client_notification";
                    smsType = BankSmsType.RegistrationDeniedUnknownClient;
                    break;
                default:
                    return;
            }

            var message = ConfigurationManager.AppSettings[messageKey];

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ConfigurationErrorsException(
                    string.Format("Не задано сообщение об отказе в регистрации (appSettings/{0})", messageKey)
                );
            }

            var parameters = new NotifyRegistrationDeniedOptions
            {
                PhoneNumber = options.PhoneNumber,
                DeniedMessage = message,
                DeniedSmsType = smsType
            };

            _security.NotifyRegistrationDenied(parameters);
        }

        public void CreateUserAndPassword(CreateUserAndPasswordOptions options)
        {
            _security.CreateUserAndPassword(options);
        }

        public void DeleteUser(string login)
        {
            _security.DeleteUser(login);
        }

        public void DisableUser(string login)
        {
            _security.DisableUser(login, true);
        }

        public void EnableUser(string login)
        {
            _security.DisableUser(login, false);
        }

        public IDictionary<string, User> BatchResolveUsersByClientId(string[] clientIds)
        {
            return _security.BatchResolveUsersByClientId(clientIds);
        }

        public IDictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones)
        {
            return _security.BatchResolveUsersByPhone(clientPhones);
        }

        public ChangeUserPhoneNumberResult ChangeUserPhoneNumber(ChangePhoneNumberOptions options)
        {
            options.ValidateAndThrow();

            var user = _security.GetUserByPhoneNumber(options.Login);

            if (user == null)
            {
                return new ChangeUserPhoneNumberResult
                {
                    Success = false,
                    Status = ChangeUserPhoneNumberStatus.UserNotFound
                };
            }

            const string MESSAGE_TEMPLATE_KEY = "security_change_phone_notification";

            var messageTemplate = ConfigurationManager.AppSettings[MESSAGE_TEMPLATE_KEY];

            if (FeaturesConfiguration.Instance.Gen25EnableSmsPasswordReset && string.IsNullOrWhiteSpace(messageTemplate))
            {
                throw new ConfigurationErrorsException(
                    string.Format("Не задано сообщение о сбросе пароля (appSettings/{0})", MESSAGE_TEMPLATE_KEY)
                );
            }

            try
            {
                _security.ChangeUserPhoneNumber(
                    new ChangeUserPhoneNumberOptions
                    {
                        UserId = user.Id,
                        NewPhoneNumber = options.NewPhoneNumber,
                        ChangedBy = options.ChangedBy
                    }
                );

                if (FeaturesConfiguration.Instance.Gen25EnableSmsPasswordReset)
                {
                    _security.ResetPassword(new ResetPasswordOptions
                        {
                            Login = options.NewPhoneNumber,
                            NotificationMessageTemplate = messageTemplate
                        });
                }
            }
            catch (UserNotFoundException)
            {
                return new ChangeUserPhoneNumberResult
                {
                    Success = false,
                    Status = ChangeUserPhoneNumberStatus.UserNotFound
                };
            }
            catch (UserAlreadyExistsException)
            {
                return new ChangeUserPhoneNumberResult
                {
                    Success = false,
                    Status = ChangeUserPhoneNumberStatus.PhoneNumberIsUsedByAnotherUser
                };
            }
            catch (SmsServiceException)
            {
                return new ChangeUserPhoneNumberResult
                {
                    Success = false,
                    Status = ChangeUserPhoneNumberStatus.FailedToSendNotification
                };
            }


            return new ChangeUserPhoneNumberResult
            {
                Success = true,
                Status = ChangeUserPhoneNumberStatus.Changed
            };
        }

        public ResetUserPasswordResult ResetUserPassword(ResetUserPasswordOptions options)
        {
            options.ValidateAndThrow();

            const string MESSAGE_TEMPLATE_KEY = "security_reset_pwd_notification";

            var messageTemplate = ConfigurationManager.AppSettings[MESSAGE_TEMPLATE_KEY];

            if (string.IsNullOrWhiteSpace(messageTemplate))
            {
                throw new ConfigurationErrorsException(
                    string.Format("Не задано сообщение о сбросе пароля (appSettings/{0})", MESSAGE_TEMPLATE_KEY)
                );
            }

            try
            {
                _security.ResetPassword(new ResetPasswordOptions
                {
                    Login = options.Login,
                    NotificationMessageTemplate = messageTemplate
                });
            }
            catch (UserNotFoundException)
            {
                return new ResetUserPasswordResult
                {
                    Success = false,
                    Status = ResetUserPasswordStatus.UserNotFound
                };
            }
            catch (SmsServiceException)
            {
                return new ResetUserPasswordResult
                {
                    Success = false,
                    Status = ResetUserPasswordStatus.FailedToSendNotification
                };
            }

            return new ResetUserPasswordResult
            {
                Success = true,
                Status = ResetUserPasswordStatus.Changed
            };
        }

        public string Echo(string message)
        {
            return string.Format("Echo: {0}", message);
        }
    }
}
