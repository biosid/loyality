using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Vtb24.Site.Security.DataAccess;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Security.OneTimePasswordService.Models.Outputs;

namespace Vtb24.Site.Security.OneTimePasswordService
{
    public class OneTimePasswordService : IOneTimePasswordService
    {
        public OneTimePasswordService(ISmsService sms, OtpConfiguration config = null)
        {
            _sms = sms;
           _cfg = config ?? new OtpConfiguration();
        }

        private readonly ISmsService _sms;
        private readonly OtpConfiguration _cfg;

        #region API

        public SendOtpResult Send(SendOtpParameters parameters)
        {
            parameters.ValidateAndThrow();

            PreventSpoofing(parameters);

            var timeout = GetTimeoutByDeliveryMeans(parameters.DeliveryMeans);

            var token = OtpToken.Create(_cfg.Length, timeout);
            token.DeliveryMeans = parameters.DeliveryMeans;
            token.To = parameters.To;
            token.ExternalId = parameters.ExternalId;
            token.OtpType = parameters.OtpType;
            token.MessageTemplate = parameters.MessageTemplate;
            token.IsFake = parameters.IsFake;

            using (var context = GetContext())
            {
                context.Tokens.Add(token);
                context.SaveChanges();
            }

            if (!parameters.IsFake)
            {
                SendToken(token);
            }

            var result = new SendOtpResult
            {
                OtpToken = token.Token,
                AttemptsRemaining = _cfg.AllowedAttempts,
                CreationTimeUtc = token.CreationTimeUtc,
                ExpirationTimeUtc = token.ExpirationTimeUtc
            };

            return result;
        }

        public RenewOtpResult Renew(RenewOtpParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var oldToken = context.Tokens.FirstOrDefault(t => t.Token == parameters.OtpToken);
                if (oldToken == null || oldToken.IsConfirmed)
                {
                    throw new InvalidOtpTokenException(parameters.OtpToken);
                }

                var allowedRenewAttempts = GetAllowedRenewAttemptsByDeliveryMeans(oldToken.DeliveryMeans);
                if (oldToken.AttemptsToRenew >= allowedRenewAttempts)
                {
                    throw new OtpRenewAttemptsExceededException(allowedRenewAttempts);
                }

                var timeout = GetTimeoutByDeliveryMeans(oldToken.DeliveryMeans);

                var token = OtpToken.Create(_cfg.Length, timeout);
                token.DeliveryMeans = oldToken.DeliveryMeans;
                token.To = oldToken.To;
                token.ExternalId = oldToken.ExternalId;
                token.OtpType = oldToken.OtpType;
                token.MessageTemplate = oldToken.MessageTemplate;
                token.AttemptsToRenew = oldToken.AttemptsToRenew + 1;
                token.IsFake = oldToken.IsFake;

                oldToken.ExpirationTimeUtc = DateTime.UtcNow;

                context.Tokens.Add(token);
                context.SaveChanges();

                if (!token.IsFake)
                {
                    SendToken(token);
                }

                var result = new RenewOtpResult
                {
                    OtpToken = token.Token,
                    AttemptsRemaining = _cfg.AllowedAttempts,
                    CreationTimeUtc = token.CreationTimeUtc,
                    ExpirationTimeUtc = token.ExpirationTimeUtc
                };

                return result;
            }
        }

        public ConfirmOtpResult Confirm(ConfirmOtpParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var token = context.Tokens.FirstOrDefault(t => t.Token == parameters.OtpToken);

                if (token == null)
                {
                    throw new InvalidOtpTokenException(parameters.OtpToken);
                }

                try
                {
                    token.AttemptsToConfirm++;

                    if (token.IsExpired())
                    {
                        throw new OtpExpiredException(token);
                    }

                    if (token.AttemptsToConfirm > _cfg.AllowedAttempts)
                    {
                        throw new OtpAllowedAttemptsExceeded(token);
                    }

                    token.IsConfirmed = token.Otp == parameters.Otp;

                    return new ConfirmOtpResult
                    {
                        Confirmed = token.IsConfirmed,
                        AttemptsRemaining = _cfg.AllowedAttempts - token.AttemptsToConfirm,
                        CreationTimeUtc = token.CreationTimeUtc,
                        ExpirationTimeUtc = token.ExpirationTimeUtc
                    };
                }
                finally
                {
                    context.SaveChanges();
                }
            }
        }

        public bool IsConfirmed(IsConfirmedOtpParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var token = context.Tokens.FirstOrDefault(t => t.Token == parameters.OtpToken);

                return token != null
                       && token.IsConfirmed
                       && token.OtpType == parameters.OtpType
                       && token.ExternalId == parameters.ExternalId;
            }
        }

        public OtpDeliveryMeans? GetDeliveryMeans(GetDeliveryMeansParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var token = context.Tokens.FirstOrDefault(t => t.Token == parameters.OtpToken);

                return token != null
                           ? token.DeliveryMeans
                           : (OtpDeliveryMeans?) null;
            }
        }

        #endregion

        private int GetTimeoutByDeliveryMeans(OtpDeliveryMeans means)
        {
            switch (means)
            {
                case OtpDeliveryMeans.Sms:
                    return _cfg.SmsTimeoutSeconds;
                case OtpDeliveryMeans.Email:
                    return _cfg.EmailTimeoutSeconds;
            }

            throw new NotSupportedException("Неподдерживаемый способ отправки OTP: " + means);
        }

        private int GetAllowedRenewAttemptsByDeliveryMeans(OtpDeliveryMeans means)
        {
            switch (means)
            {
                case OtpDeliveryMeans.Sms:
                    return _cfg.SmsAllowedRenewAttempts;
                case OtpDeliveryMeans.Email:
                    return _cfg.EmailAllowedRenewAttempts;
            }

            throw new NotSupportedException("Неподдерживаемый способ отправки OTP: " + means);
        }

        private void SendToken(OtpToken token)
        {
            var message = string.Format(token.MessageTemplate, token.Otp);

            switch (token.DeliveryMeans)
            {
                case OtpDeliveryMeans.Sms:
                    _sms.Send(token.To, message);
                    break;

                case OtpDeliveryMeans.Email:
                    SendTokenByEmail(token.To, message);
                    break;

                default:
                    throw new NotSupportedException("Неподдерживаемый способ отправки OTP: " + token.DeliveryMeans);
            }
        }

        private void SendTokenByEmail(string to, string message)
        {
            var messageParts = message.Split(new[] { '|' }, 2);
            var subject = messageParts.Length == 2 ? messageParts[0] : string.Empty;
            var body = messageParts.Length == 2 ? messageParts[1] : message;

            using (var client = new SmtpClient(_cfg.SmtpHost, _cfg.SmtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_cfg.SmtpUser, _cfg.SmtpPassword)
            })
            {
                var mail = new MailMessage(_cfg.EmailFrom, to)
                {
                    Subject = subject,
                    IsBodyHtml = false,
                    Body = body
                };

                client.Send(mail);
            }
        }

        private void PreventSpoofing(SendOtpParameters parameters)
        {
            using (var context = GetContext())
            {
                var stats = context.Tokens
                                   .Where(s => s.OtpType == parameters.OtpType &&
                                          s.To == parameters.To)
                                   .OrderByDescending (s => s.CreationTimeUtc);


                if (_cfg.GetSimilarTimeoutSeconds(parameters.OtpType) != 0)
                {
                    var lastAttempt = stats.FirstOrDefault ();

                    var interval = lastAttempt != null
                                       ? (int?)(DateTime.UtcNow - lastAttempt.CreationTimeUtc).TotalSeconds
                                       : null;

                    var isTooFrequent =
                        interval.HasValue &&
                        interval < _cfg.GetSimilarTimeoutSeconds(parameters.OtpType);

                    if (isTooFrequent)
                    {
                        throw new TooFrequentOtpSendException(_cfg.GetSimilarTimeoutSeconds (parameters.OtpType),
                                                              lastAttempt.CreationTimeUtc);
                    }
                }

                if (_cfg.GetSimilarThresholdIntervalInSeconds (parameters.OtpType) != 0 &&
                    _cfg.GetSimilarThresholdAttempts(parameters.OtpType) != 0)
                {
                    var intervalStart = DateTime.UtcNow.AddSeconds(-(_cfg.GetSimilarThresholdIntervalInSeconds(parameters.OtpType)));

                    var attemptsCount = stats.Count(s => s.CreationTimeUtc >= intervalStart);

                    var isTooMany =  attemptsCount > _cfg.GetSimilarThresholdAttempts(parameters.OtpType);
                    
                    if (isTooMany)
                    {
                        throw new TooManyOtpSendAttemptsException(
                            attemptsCount,
                            _cfg.GetSimilarThresholdIntervalInSeconds(parameters.OtpType),
                            DateTime.UtcNow.AddSeconds(_cfg.GetSimilarThresholdIntervalInSeconds(parameters.OtpType))
                            );
                    }
                }
            }
        }

        private IOtpDataAccess GetContext()
        {
            return new SecurityServiceDbContext();
        }
    }
}