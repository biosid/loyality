namespace Vtb24.Site.Services.ServiceLogger
{
    public class NoneServiceLogger : IServiceLogger
    {
        public void LogRequest(string serviceName, object request, object correlationState = null)
        {
        }

        public void LogResponse(string serviceName, object response, object correlationState = null)
        {
        }
    }
}
