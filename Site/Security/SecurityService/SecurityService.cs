using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models.Outputs;
using WebMatrix.WebData;
using Vtb24.Site.Security.DataAccess;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;

namespace Vtb24.Site.Security.SecurityService
{
    public class SecurityService : ISecurityService
    {
        public const int MAX_BATCH_LENGTH = 500;
        public const string CHANGE_PASSWORD_OTP_TYPE = "sec_reset_pwd";

        public SecurityService(ISmsService sms, IBankSmsService bankSms, IOneTimePasswordService otp, ISecurityTokenService tokens, SecurityConfiguration config = null)
        {
            _sms = sms;
            _bankSms = bankSms;
            _otp = otp;
            _tokens = tokens;
            _cfg = config ?? new SecurityConfiguration();

            lock (Lock)
            {
                if (!_initialized)
                {
                    using (var context = new SecurityServiceDbContext())
                    {
                        context.Database.Initialize(false);
                        WebSecurity.InitializeDatabaseConnection(
                            connectionStringName: SecurityServiceDbContext.GetConnectionString(),
                            userTableName: "Users",
                            userIdColumn: "Id",
                            userNameColumn: "PhoneNumber",
                            autoCreateTables: true
                            );
                        _initialized = true;
                    }
                }
            }
        }

        private static bool _initialized;
        private static readonly object Lock = new object();
        private readonly ISmsService _sms;
        private readonly IBankSmsService _bankSms;
        private readonly IOneTimePasswordService _otp;
        private readonly ISecurityTokenService _tokens;
        private readonly SecurityConfiguration _cfg;

        public bool OtpViaEmailEnabled
        {
            get { return _cfg.EnableOtpViaEmail; }
        }

        public ClientPrincipal GetPrincipal()
        {
            var request = HttpContext.Current.Request;
            var anonId = request.AnonymousID;
            var ip = GetClientIpAddress(request);

            var principal = new ClientPrincipal
            {
                ClientAnonymousId = anonId,
                ClientIp = ip
            };

            using (var context = GetContext())
            {
                if (WebSecurity.IsAuthenticated)
                {
                    var account = context.Users.FirstOrDefault(a => a.PhoneNumber == WebSecurity.CurrentUserName);
                    if (account == null)
                    {
                        throw new UserNotFoundException(WebSecurity.CurrentUserName);
                    }

                    if (account.IsDisabled)
                    {
                        throw new UserDisabledException(account);
                    }

                    principal.ClientId = account.ClientId;
                    principal.PhoneNumber = account.PhoneNumber;
                }
            }
            return principal;
        }

        public User GetUserByPhoneNumber (string phone)
        {
            using (var context = GetContext())
            {
                var account = context.Users.FirstOrDefault(u => u.PhoneNumber == phone);

                return account;
            }
        }

        public User[] GetUsersByClientId(string clientId, SecurityPagingSettings paging = null)
        {
            paging = paging ?? new SecurityPagingSettings();
            using (var context = GetContext())
            {
                var users = context.Users
                    .Where(u => u.ClientId == clientId)
                    .OrderBy(u=>u.Id)
                    .Skip(paging.Skip)
                    .Take(paging.Take)
                    .ToArray();
                return users;
            }
        }

        public bool Login(string login, string password)
        {
            // шаг 1. проверяем, не превышено ли кол-во попыток ввода пароля
            var isLocked = WebSecurity.IsAccountLockedOut(
                    login, 
                    _cfg.AllowedPasswordAttempts - 1,
                    _cfg.LockoutIntervalInMinutes * 60
                );
            if (isLocked)
            {
                // сценарий временной блокировки
                throw new UserLockedOutException(_cfg.LockoutIntervalInMinutes);
            }

            //  шаг 2. Проверяем логин/пароль
            using (var context = GetContext())
            {
                var account = context.Users.FirstOrDefault(a => a.PhoneNumber == login);

                var result = Membership.ValidateUser(login, password);
                if (account == null || !result)
                {
                    // получение юзера и проверка пароля объединены не случайно :)
                    return false;
                }

                // шаг 3. Проверяем статус блокировки
                if (account.IsDisabled)
                {
                    // сценарий заблокированной учётной записи
                    throw new UserDisabledException(account);
                }

                // шаг 4. Проверяем необходимость смены пароля
                if (!account.IsPasswordSet)
                {
                    // сценарий принудительной смены пароля
                    throw new PasswordNotSetException();
                }
            }
            // шаг 5. Устанавливаем куку
            FormsAuthentication.SetAuthCookie(login, true);
                
            return true;
        }

        public bool ChangePassword(ChangePasswordOptions options)
        {
            options.ValidateAndThrow();

            using (var context = GetContext())
            {
                var account = context.Users.FirstOrDefault(a => a.PhoneNumber == options.Login);

                if (account == null)
                {
                    return false;
                }

                var result = WebSecurity.ChangePassword(options.Login, options.Password, options.NewPassword);

                if (result)
                {
                    account.IsPasswordSet = true;
                    context.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public void ResetPassword(ResetPasswordOptions options)
        {
            options.ValidateAndThrow();

            using (var context = GetContext())
            {
                var account = context.Users.FirstOrDefault(a => a.PhoneNumber == options.Login);

                if (account == null)
                {
                    throw new UserNotFoundException(options.Login);
                }

                var newPassword = GenerateRandomPassword(_cfg.TempPasswordLength);

                var token = WebSecurity.GeneratePasswordResetToken(options.Login);
                var result = WebSecurity.ResetPassword(token, newPassword);

                if (!result)
                {
                    throw new SecurityServiceException("Ошибка при сбросе пароля");
                }

                account.IsPasswordSet = false;
                context.SaveChanges();

                var message = string.Format(options.NotificationMessageTemplate, newPassword, options.Login);

                _sms.Send(account.PhoneNumber, message);
            }
        }

        public SendChangePasswordOtpResult SendChangePasswordOtp(SendChangePasswordOtpOptions options)
        {
            options.ValidateAndThrow();

            using (var context = GetContext())
            {
                var user = context.Users.FirstOrDefault(u => u.PhoneNumber == options.Login);

                if (user != null && user.IsDisabled)
                {
                    throw new UserDisabledException(user);
                }

                var otpParams = new SendOtpParameters
                {
                    OtpType = CHANGE_PASSWORD_OTP_TYPE,
                    ExternalId = options.Login,
                    IsFake = (user == null),
                };

                if (!_cfg.EnableOtpViaEmail || options.ForceSms)
                {
                    otpParams.DeliveryMeans = OtpDeliveryMeans.Sms;
                    otpParams.To = options.Login;
                    otpParams.MessageTemplate = options.SmsOtpMessageTemplate;
                }
                else if (string.IsNullOrWhiteSpace(options.Email))
                {
                    if (user != null && _cfg.PasswordChangeOtpSmsLimit >= 0)
                    {
                        if (user.OtpViaSmsCount >= _cfg.PasswordChangeOtpSmsLimit)
                        {
                            throw new OtpViaSmsLimitReached();
                        }

                        ++user.OtpViaSmsCount;
                    }

                    otpParams.DeliveryMeans = OtpDeliveryMeans.Sms;
                    otpParams.To = options.Login;
                    otpParams.MessageTemplate = options.SmsOtpMessageTemplate;
                }
                else
                {
                    otpParams.DeliveryMeans = OtpDeliveryMeans.Email;
                    otpParams.To = options.Email;
                    otpParams.MessageTemplate = options.EmailOtpMessageTemplate;
                }


                var otpResult = _otp.Send(otpParams);

                context.SaveChanges();

                return new SendChangePasswordOtpResult
                {
                    CreationTimeUtc = otpResult.CreationTimeUtc,
                    ExpirationTimeUtc = otpResult.ExpirationTimeUtc,
                    OtpToken = otpResult.OtpToken,
                    DeliveryMeans = otpParams.DeliveryMeans
                };
            }
        }

        public void ChangePasswordWithOtp(ChangePasswordWithOtpOptions options)
        {
            options.ValidateAndThrow();

            using (var context = GetContext())
            {
                var otpParams = new IsConfirmedOtpParameters
                {
                    OtpType = CHANGE_PASSWORD_OTP_TYPE,
                    OtpToken = options.OtpToken,
                    ExternalId = options.Login
                };
                var confirmed = _otp.IsConfirmed(otpParams);

                if (!confirmed)
                {
                    throw new InvalidChangePasswordOtpException(options.Login);
                }

                var token = WebSecurity.GeneratePasswordResetToken(options.Login);
                WebSecurity.ResetPassword(token, options.NewPassword);

                var account = context.Users.First(a => a.PhoneNumber == options.Login);
                account.IsPasswordSet = true;
                context.SaveChanges();

                // сбросим таймстэмп ввода неправильного пароля, иначе не получится залогиниться
                context.ResetLastPasswordFailureDate(account.Id);
            }
        }

        public void CreateUser(CreateClientAccountOptions options)
        {
            options.ValidateAndThrow();
            AssertUserExists(options.PhoneNumber);

            var password = GenerateRandomPassword(_cfg.TempPasswordLength);

            WebSecurity.CreateUserAndAccount(
                options.PhoneNumber,
                password,
                new
                {
                    options.ClientId,
                    IsDisabled = false,
                    IsPasswordSet = false,
                    RegistrationDate = DateTime.Now
                }
            );

            try
            {
                if (_cfg.DisableBankSms)
                {
                    var message = string.Format(options.WelcomeMessageTemplate, options.PhoneNumber, password);
                    _sms.Send(options.PhoneNumber, message);
                }
                else
                {
                    _bankSms.Send(options.WelcomeSmsType, options.PhoneNumber, password);
                }
            }
            catch
            {
                DeleteUser(options.PhoneNumber);
                throw;
            }
        }

        public void CreateUserAndPassword(CreateUserAndPasswordOptions options)
        {
            options.ValidateAndThrow();
            AssertUserExists(options.PhoneNumber);

            WebSecurity.CreateUserAndAccount(
                options.PhoneNumber,
                options.Password,
                new
                {
                    options.ClientId,
                    IsDisabled = false,
                    IsPasswordSet = !options.ForcePasswordChange,
                    RegistrationDate = DateTime.Now
                }
            );
        }

        public void DeleteUser(string login)
        {
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(login);
            Membership.Provider.DeleteUser(login, true);
        }

        public void DisableUser(string login, bool disable)
        {
            using (var context = GetContext())
            {
                var account = context.Users.SingleOrDefault(a => a.PhoneNumber == login);

                if (account == null)
                {
                    throw new UserNotFoundException(login);
                }

                account.IsDisabled = disable;

                context.SaveChanges();
            }
        }

        public IDictionary<string, User> BatchResolveUsersByClientId(string[] clientIds)
        {
            AssertBatchSize(clientIds.Length);

            using (var context = GetContext())
            {
                var users = context.Users.Where(u => clientIds.Contains(u.ClientId)).ToArray();

                var result = clientIds.Distinct().ToDictionary(id => id, id => (User)null);

                foreach (var user in users)
                {
                    result[user.ClientId] = user;
                }

                return result;
            }
        }

        public IDictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones)
        {
            AssertBatchSize(clientPhones.Length);

            using (var context = GetContext())
            {
                var users = context.Users.Where(u => clientPhones.Contains(u.PhoneNumber)).ToArray();

                var result = clientPhones.Distinct().ToDictionary(p => p, p => (User) null);

                foreach (var user in users)
                {
                    result[user.PhoneNumber] = user;
                }

                return result;
            }
        }

        public void ChangeUserPhoneNumber(ChangeUserPhoneNumberOptions options)
        {
            options.ValidateAndThrow();

            using (var context = GetContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == options.UserId);

                if (user == null)
                {
                    throw new UserNotFoundException(options.UserId);
                }

                var phoneIsNotAvailable = context.Users.Any(u => u.PhoneNumber == options.NewPhoneNumber);

                if (phoneIsNotAvailable)
                {
                    throw new UserAlreadyExistsException(options.NewPhoneNumber);
                }

                var change = new PhoneNumberChange
                {
                    ChangeTime = DateTime.Now,
                    UserId = user.Id,
                    OldPhoneNumber = user.PhoneNumber,
                    NewPhoneNumber = options.NewPhoneNumber,
                    ChangedBy = options.ChangedBy
                };
                context.PhoneNumberChangeHistory.Add(change);
                user.PhoneNumber = options.NewPhoneNumber;

                context.SaveChanges();
            }
        }

        public PhoneNumberChangeHistory GetChangeUserPhoneNumberHistory(int userId, SecurityPagingSettings paging = null)
        {
            paging = paging ?? new SecurityPagingSettings();

            using (var context = GetContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                var query = context
                    .PhoneNumberChangeHistory
                    .Where(c=>c.UserId == user.Id)
                    .OrderByDescending(c=>c.ChangeTime);

                var history = query.Skip(paging.Skip).Take(paging.Take).ToArray();
                var total = query.Count();

                var result = new PhoneNumberChangeHistory
                {
                    User = user,
                    History = history,
                    TotalCount = total,
                    SecurityPagingSettings = paging
                };

                return result;
            }
        }

        public void NotifyRegistrationDenied(NotifyRegistrationDeniedOptions options)
        {
            options.ValidateAndThrow();

            if (_cfg.DisableBankSms)
            {
                _sms.Send(options.PhoneNumber, options.DeniedMessage);
            }
            else
            {
                _bankSms.Send(options.DeniedSmsType, options.PhoneNumber, string.Empty);
            }
        }

        public void Logout()
        {
            if (WebSecurity.IsAuthenticated)
            {
                var parameters = new InvalidateSecurityTokenParameters
                {
                    PrincipalId = WebSecurity.CurrentUserName
                };

                _tokens.Invalidate(parameters);
            }

            WebSecurity.Logout();
        }

        private void AssertUserExists(string phone)
        {
            using (var context = GetContext())
            {
                var accountExists = context.Users.Any(a => a.PhoneNumber == phone);

                if (accountExists)
                {
                    throw new UserAlreadyExistsException(phone);
                }
            }
        }

        private void AssertBatchSize(int size)
        {
            if (size > MAX_BATCH_LENGTH)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Превышен максимальный размер пакета (максимум: {0}, получено: {1})",
                        MAX_BATCH_LENGTH,
                        size
                    )
                );
            }
        }

        private ISecurityDataAccess GetContext()
        {
            return new SecurityServiceDbContext();
        }

        /// <summary>
        /// Получить IP пользователя, с учётом возможных баллансировщиков
        /// </summary>
        private static string GetClientIpAddress(HttpRequest request)
        {
            string ip = null;

            if (request.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED_FOR"))
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new[] { ',', '\0', ' ', '\t' }).FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip == "::1" ? "127.0.0.1" : ip;
        }

        /// <summary>
        /// Генерация пароля заданой длины. 
        /// В пароле обязательно присутствуют латинские буквы и цифры.
        /// Из пароля исключены похожие символы l, 1, I, O и 0.
        /// </summary>
        private static string GenerateRandomPassword(int length)
        {
            // генерация пароля
            var bytesLength = (int) Math.Ceiling((length / 4d) * 3);
            var bytes = new byte[bytesLength];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            var randomStr = Convert.ToBase64String(bytes).Substring(0, length); // строка формата [A-Za-z0-9+/]
            var pasword = randomStr.ToCharArray();

            // постаброботка (удаление нежелательных символов, добавление недостающих цифр и букв)
            var seed = new byte[4];
            rng.GetBytes(seed);
            var rnd = new Random(BitConverter.ToInt32(seed, 0));

            const string ALPHA_DICT = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            const string DIGIT_DICT = "3456789";
            const string FULL_DICT = ALPHA_DICT + DIGIT_DICT;
            var digits = 0;
            var alphas = 0;

            for (var i = 0; i < length; i++)
            {
             // заменяем запрещённые символы разрешёнными
             switch (pasword[i])
             {
                 case '/': case '+': case '1': case 'l': case 'I': case '0': case 'O':
                     pasword[i] = FULL_DICT[rnd.Next(0, FULL_DICT.Length - 1)];
                 break;
             }
             // заодно, собираем статистику
             if (char.IsDigit(pasword[i]))
             {
                 digits++;
             }
             else
             {
                 alphas++;
             }
            }

            // добавляем цифру, если нет
            if (digits == 0)
            {
             pasword[rnd.Next(0, length - 1)] = DIGIT_DICT[rnd.Next(0, DIGIT_DICT.Length -1)];
            }
            // добавляем букву, если нет
            if (alphas == 0)
            {
             pasword[rnd.Next(0, length - 1)] = ALPHA_DICT[rnd.Next(0, ALPHA_DICT.Length -1)];
            }

            // спасибо
            return new string(pasword);
        }
    }
}