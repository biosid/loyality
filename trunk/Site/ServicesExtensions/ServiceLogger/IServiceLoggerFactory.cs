namespace Vtb24.ServicesExtensions.ServiceLogger
{
    public interface IServiceLoggerFactory
    {
        /// <summary>
        /// Creates logger instance for service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="side">log side (Client or Service)</param>
        /// <param name="properties">additional properties to log with every request and response</param>
        /// <returns>logger</returns>
        IServiceLogger Create(string serviceName, ServiceLogSide side, object properties = null);

        /// <summary>
        /// Creates service side logger for specified service
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="properties">additional properties to log with every request and response</param>
        /// <returns>logger</returns>
        IServiceLogger ForService<TService>(object properties = null);

        /// <summary>
        /// Creates client side logger for specified service
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="properties">additional properties to log with every request and response</param>
        /// <returns>logger</returns>
        IServiceLogger ForClient<TService>(object properties = null);
    }
}
