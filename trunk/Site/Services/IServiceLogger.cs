namespace Vtb24.Site.Services
{
    public interface IServiceLogger
    {
        /// <summary>
        /// Logs service request to be sent.
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="request">request</param>
        /// <param name="correlationState">unique request/response identifier</param>
        void LogRequest(string serviceName, object request, object correlationState = null);

        /// <summary>
        /// Logs service response received
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="response">response</param>
        /// <param name="correlationState">unique request/response identifier</param>
        void LogResponse(string serviceName, object response, object correlationState = null);
    }
}
