using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Vtb24.ServicesExtensions.ServiceLogger;
using Vtb24.Site.Security;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.SmsService.Models.Exceptions;

namespace Vtb24.Site.Services.SmsService
{
    public class SmsService : ISmsService
    {
        public SmsService(IServiceLoggerFactory serviceLogFactory)
        {
            _serviceLog = serviceLogFactory.ForService<SmsService>();
            _gatewayLog = serviceLogFactory.Create("SmsGateway", ServiceLogSide.Client);
        }

        private readonly IServiceLogger _serviceLog;
        private readonly IServiceLogger _gatewayLog;

        public void Send(string phone, string message)
        {
            var requestId = Guid.NewGuid();

            _serviceLog.Request(new { Phone = phone, Message = message }, requestId);

            var response = GetGatewayResponse(EndpointTemplate, phone, message);

            if (response == null || IsServerError(response))
            {
                var backupUrlTemplate = BackupEndpointTemplate;

                if (!string.IsNullOrEmpty(backupUrlTemplate))
                {
                    response = GetGatewayResponse(backupUrlTemplate, phone, message);
                }
            }

            string errorDescription;

            if (!IsGatewayResponseSucceeded(response, out errorDescription))
            {
                _serviceLog.Reply(new { Success = false, ErrorDescription = errorDescription }, requestId);

                throw new SmsServiceException(errorDescription);
            }

            _serviceLog.Reply(new { Success = true }, requestId);
        }

        private static bool IsGatewayResponseSucceeded(HttpResponseMessage response, out string errorDescription)
        {
            if (response == null)
            {
                errorDescription = "Не удалось отправить запрос в SMS-шлюз";
                return false;
            }
            
            if (!response.IsSuccessStatusCode)
            {
                errorDescription = "SMS-шлюз вернул ошибку: " + response.Content.ReadAsStringAsync().Result;
                return false;
            }

            errorDescription = null;
            return true;
        }

        private HttpResponseMessage GetGatewayResponse(string urlTemplate, string phone, string message)
        {
            var url = string.Format(urlTemplate, phone, HttpUtility.UrlEncode(message));

            var requestId = Guid.NewGuid();

            _gatewayLog.Request(url, requestId);

            var client = new HttpClient
            {
                Timeout = HttpTimeout
            };

            try
            {
                try
                {
                    var response = client.GetAsync(url).Result;

                    _gatewayLog.Reply(response, requestId);

                    return response;
                }
                catch (AggregateException aex)
                {
                    aex.Handle(ex =>
                        {
                            if (ex is TaskCanceledException)
                            {
                                _gatewayLog.Reply("произошел таймаут ожидания ответа", requestId);
                                return true;
                            }

                            return false;
                        });
                    return null;
                }
            }
            catch (Exception ex)
            {
                _gatewayLog.Reply(string.Format("произошла ошибка: {0}", ex), requestId);
                return null;
            }
        }

        private static string EndpointTemplate { get { return AppSettingsHelper.String("sms_gateway_endpoint", null); } }

        private static string BackupEndpointTemplate { get { return AppSettingsHelper.String("sms_gateway_backup_endpoint", null); } }

        private static TimeSpan HttpTimeout { get { return TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * AppSettingsHelper.Int("sms_gateway_timeout", 30000)); } }

        private static bool IsServerError(HttpResponseMessage response)
        {
            var code = (int) response.StatusCode;
            return code >= 500 && code < 600;
        }
    }
}
