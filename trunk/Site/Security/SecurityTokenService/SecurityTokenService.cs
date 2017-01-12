using System;
using System.Linq;
using Vtb24.Site.Security.DataAccess;
using Vtb24.Site.Security.Infrastructure;
using Vtb24.Site.Security.SecurityTokenService.Models;
using Vtb24.Site.Security.SecurityTokenService.Models.Exceptions;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.Security.SecurityTokenService.Models.Outputs;

namespace Vtb24.Site.Security.SecurityTokenService
{
    public class SecurityTokenService : ISecurityTokenService
    {
        protected int OtpTimeoutMinutes
        {
            get { return AppSettingsHelper.Int("security_token_timeout_minutes", 24 * 60); }
        }

        private ISecurityTokenDataAccess GetContext()
        {
            return new SecurityServiceDbContext();
        }

        #region API

        public CreateSecurityTokenResult Create(CreateSecurityTokenParameters parameters)
        {
            parameters.ValidateAndThrow();

            var token = SecurityToken.Create(timeoutMinutes: OtpTimeoutMinutes);
            token.PrincipalId = parameters.PrincipalId;
            token.ExternalId = parameters.ExternalId;
            token.CategoryId = parameters.CategoryId;

            using (var context = GetContext())
            {
                context.SecurityTokens.Add(token);
                context.SaveChanges();
            }

            var result = new CreateSecurityTokenResult
            {
                SecurityToken = token.Token,
                ExternalId = token.ExternalId,
                CreationTimeUtc = token.CreationTimeUtc,
                ExpirationTimeUtc = token.ExpirationTimeUtc
            };

            return result;
        }

        public ValidateSecurityTokenResult Validate(ValidateSecurityTokenParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var token = context.SecurityTokens.FirstOrDefault(t => t.Token == parameters.SecurityToken);

                if (token == null)
                {
                    throw new InvalidSecurityTokenException(parameters.SecurityToken);
                }

                var result = new ValidateSecurityTokenResult
                {
                    IsValid =  token.ExpirationTimeUtc > DateTime.UtcNow,
                    PrincipalId = token.PrincipalId,
                    ExternalId = token.ExternalId,
                    CategoryId = token.CategoryId,
                    CreationTimeUtc = token.CreationTimeUtc,
                    ExpirationTimeUtc = token.ExpirationTimeUtc
                };
                return result;
            }
        }

        public void Invalidate(InvalidateSecurityTokenParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var context = GetContext())
            {
                var tokens = context.SecurityTokens.Where(t => t.PrincipalId == parameters.PrincipalId);

                if (tokens != null && tokens.Any())
                {
                    var now = DateTime.UtcNow;

                    foreach (var token in tokens)
                    {
                        token.ExpirationTimeUtc = now;
                    }

                    context.SaveChanges();
                }
            }
        }

        #endregion
    }
}