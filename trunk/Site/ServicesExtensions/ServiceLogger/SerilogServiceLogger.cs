using System;
using System.Security.Claims;
using Serilog;
using Vtb24.Logging;

namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class SerilogServiceLogger : IServiceLogger
    {
        private const string REQUEST_TEMPLATE = @"
BEGIN REQUEST #{RequestId}
Properties: {@SvcProperties}
Context: {@SvcMsgContext}
{Request}
END REQUEST
";

        private const string RESPONSE_TEMPLATE = @"
BEGIN REPLY #{RequestId}
Properties: {@SvcProperties}
Context: {@SvcMsgContext}
{Reply}
END REPLY
";

        public SerilogServiceLogger(string serviceName, ServiceLogSide side, object properties)
        {
            _log = SerilogLoggers.ServiceLogger;

            _properties = new
            {
                ServiceName = serviceName,
                ServiceSide = side,
                Other = properties
            };
        }

        private readonly ILogger _log;
        private readonly object _properties;

        public void Request(object request, Guid? requestId)
        {
            _log.Information(REQUEST_TEMPLATE, requestId, _properties, GetContext(), request);
        }

        public void Reply(object reply, Guid? requestId)
        {
            _log.Information(RESPONSE_TEMPLATE, requestId, _properties, GetContext(), reply);
        }

        private static object GetContext()
        {
            return new
            {
                HttpRequestId = HttpHelpers.GetRequestId(),
                UserName = ClaimsPrincipal.Current.Identity.Name
            };
        }
    }
}
