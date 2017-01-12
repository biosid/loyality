namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public class SerilogServiceLoggerFactory : ServiceLoggerFactory
    {
        public override IServiceLogger Create(string serviceName, ServiceLogSide side, object properties)
        {
            return new SerilogServiceLogger(serviceName, side, properties);
        }
    }
}
