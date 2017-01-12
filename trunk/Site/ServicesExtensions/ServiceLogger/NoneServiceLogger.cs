using System;

namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class NoneServiceLogger : IServiceLogger
    {
        public void Request(object request, Guid? requestId = null)
        {
        }

        public void Reply(object reply, Guid? requestId = null)
        {
        }
    }
}
