namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class NoneServiceLoggerFactory : ServiceLoggerFactory
    {
        public override IServiceLogger Create(string serviceName, ServiceLogSide side, object properties)
        {
            return new NoneServiceLogger();
        }
    }
}
