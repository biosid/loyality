namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public abstract class ServiceLoggerFactory : IServiceLoggerFactory
    {
        public abstract IServiceLogger Create(string serviceName, ServiceLogSide side, object properties);

        public IServiceLogger ForService<TService>(object properties)
        {
            return Create(typeof (TService).FullName, ServiceLogSide.Service, properties);
        }

        public IServiceLogger ForClient<TService>(object properties)
        {
            return Create(typeof(TService).FullName, ServiceLogSide.Client, properties);
        }
    }
}
