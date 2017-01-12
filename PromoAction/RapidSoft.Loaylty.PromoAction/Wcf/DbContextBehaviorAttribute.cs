namespace RapidSoft.Loaylty.PromoAction.Wcf
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using RapidSoft.Loaylty.PromoAction.Repositories;

    /// <summary>
    /// Атрибут добавляющий/реализующий управление временем жизни контекстов.
    /// </summary>
    public class DbContextBehaviorAttribute : Attribute, IServiceBehavior, IParameterInspector
    {
        /// <summary>
        /// Признак отключаения создание прокси EF.
        /// </summary>
        private readonly bool disableProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBehaviorAttribute"/> class.
        /// </summary>
        public DbContextBehaviorAttribute()
        {
            this.disableProxy = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="disableProxy">
        /// Признак отключаения создание прокси EF.
        /// </param>
        public DbContextBehaviorAttribute(bool disableProxy)
        {
            this.disableProxy = disableProxy;
        }

        #region IServiceBehavior

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, 
        /// message or parameter interceptors, security extensions, and other custom extension objects.
        /// Добавляет себя в коллекцию <see cref="DispatchOperation.ParameterInspectors"/>.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
                    {
                        operation.ParameterInspectors.Add(this);
                    }
                }
            }
        }
        #endregion

        #region IParameterInspector

        /// <summary>
        /// Called before client calls are sent and after service responses are returned.
        /// </summary>
        /// <returns>
        /// The correlation state that is returned as the parameter in <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.AfterCall(System.String,System.Object[],System.Object,System.Object)"/>. 
        /// Return null if you do not intend to use correlation state.
        /// </returns>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="inputs">The objects passed to the method by the client.</param>
        public object BeforeCall(string operationName, object[] inputs)
        {
            if (this.disableProxy)
            {
                MechanicsDbContext.Get().Configuration.ProxyCreationEnabled = !this.disableProxy;
            }

            return null;
        }

        /// <summary>
        /// Called after client calls are returned and before service responses are sent.
        /// Выполняет вызов <see cref="MechanicsDbContext.DisposeContext"/>.
        /// </summary>
        /// <param name="operationName">The name of the invoked operation.</param>
        /// <param name="outputs">Any output objects.</param>
        /// <param name="returnValue">The return value of the operation.</param>
        /// <param name="correlationState">
        /// Any correlation state returned from the <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.BeforeCall(System.String,System.Object[])"/> method, 
        /// or null. </param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            MechanicsDbContext.DisposeContext();
        }
        #endregion
    }
}