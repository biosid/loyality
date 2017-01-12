using System;
using System.Security.Cryptography;
using System.Text;
using Vtb24.ServicesExtensions.ServiceLogger;
using Vtb24.Site.Security;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Processing;
using Vtb24.Site.Services.Processing.Models.Inputs;

namespace Vtb24.Site.Services.SmsBallance
{
    public class SmsBallanceService : ISmsBallanceService
    {
        public SmsBallanceService(IProcessing processing, ISecurityService securityService, IServiceLoggerFactory serviceLogFactory)
        {
            _processing = processing;
            _securityService = securityService;
            _serviceLog = serviceLogFactory.ForService<SmsBallanceService>();
        }

        private readonly ISecurityService _securityService;
        private readonly IProcessing _processing;
        private readonly IServiceLogger _serviceLog;

        public decimal GetAccountBallance(string phoneNumber)
        {
            var requestId = Guid.NewGuid();

            _serviceLog.Request(new { Method = "GetAccountBalance", PhoneNumber = phoneNumber }, requestId);

            try
            {
                var client = _securityService.GetUserByPhoneNumber(phoneNumber);

                if (client == null)
                {
                    _serviceLog.Reply(new { Success = false, ErrorDescription = "пользователь не найден" });

                    throw new UserNotFoundException(phoneNumber);
                }

                var options = new GetBalanceParameters { ClientId = client.ClientId };
                var ballance = _processing.GetBalance(options);

                _serviceLog.Reply(new { Success = true, Balance = ballance });

                return ballance;
            }
            catch (Exception ex)
            {
                _serviceLog.Reply(new { Success = false, ErrorDescription = string.Format("произошла ошибка: {0}", ex) }, requestId);
                throw;
            }
        }

        public bool ValidateRequestHash(string checkString, string hash)
        {
            var requestId = Guid.NewGuid();

            _serviceLog.Request(new { Method = "ValidateRequestHash", CheckString = checkString, Hash = hash }, requestId);

            try
            {
                var privateKey = AppSettingsHelper.String("sms_gateway_hash_key", String.Empty);
                var privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);

                var isValid = true;

                using (var hmac = new HMACSHA256(privateKeyBytes))
                {
                    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(checkString));
                    var hashString = Convert.ToBase64String(hashBytes);

                    if (!string.Equals(hashString, hash, StringComparison.InvariantCulture))
                    {
                        isValid = false;
                    }
                }

                _serviceLog.Reply(new { Success = true, IsValid = isValid }, requestId);

                return isValid;
            }
            catch (Exception ex)
            {
                _serviceLog.Reply(new { Success = false, ErrorDescription = string.Format("произошла ошибка: {0}", ex) }, requestId);
                throw;
            }
        }
    }
}
