using System;

namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public interface IServiceLogger
    {
        /// <summary>
        /// Logs request
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="requestId">unique request/response identifier</param>
        void Request(object request, Guid? requestId = null);

        /// <summary>
        /// Logs reply
        /// </summary>
        /// <param name="reply">reply</param>
        /// <param name="requestId">unique request/response identifier</param>
        void Reply(object reply, Guid? requestId = null);
    }
}
