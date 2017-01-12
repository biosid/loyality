using System;
using System.Security.Claims;
using Vtb24.Logging;
using log4net;

namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class Log4NetServiceLogger : IServiceLogger
    {
        public Log4NetServiceLogger(string serviceName, ServiceLogSide side, object properties)
        {
            _properties =
                Environment.NewLine + Constants.SERIVCE_NAME_PROPERTY_NAME + " = " + serviceName +
                Environment.NewLine + Constants.SERIVCE_SIDE_PROPERTY_NAME + " = " + side;

            foreach (var property in properties.GetType().GetProperties())
            {
                _properties += Environment.NewLine + property.Name + " = " + property.GetValue(properties);
            }
        }

        private readonly ILog _log = LogManager.GetLogger(typeof (Log4NetServiceLogger));

        private readonly string _properties;

        public void Request(object request, Guid? requestId = null)
        {
            var requestIdStr = requestId.HasValue ? requestId.Value.ToString("D") : "null";

            _log.Info(Environment.NewLine + "BEGIN REQUEST #" + requestIdStr + _properties + GetContext() +
                      Environment.NewLine + request + Environment.NewLine + "END REQUEST" + Environment.NewLine);
        }

        public void Reply(object reply, Guid? requestId = null)
        {
            var requestIdStr = requestId.HasValue ? requestId.Value.ToString("D") : "null";

            _log.Info(Environment.NewLine + "BEGIN REPLY #" + requestIdStr + _properties + GetContext() +
                      Environment.NewLine + reply + Environment.NewLine + "END REPLY" + Environment.NewLine);
        }

        private static string GetContext()
        {
            return
                Environment.NewLine + "HTTP request ID = " + HttpHelpers.GetRequestId() +
                Environment.NewLine + "User identity name = " + ClaimsPrincipal.Current.Identity.Name;
        }
    }
}
