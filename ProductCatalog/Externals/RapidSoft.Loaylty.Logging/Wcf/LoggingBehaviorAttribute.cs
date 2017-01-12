namespace RapidSoft.Loaylty.Logging.Wcf
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Атрибут реализующий логирование и обработку ошибок вызовов wcf-сервисов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class LoggingBehaviorAttribute : Attribute, IServiceBehavior, IErrorHandler, IParameterInspector
    {
        private static readonly ParameterSerializer Serializer;

        static LoggingBehaviorAttribute()
        {
            Serializer = ParameterSerializer.Build();
        }

        private readonly ILog log = LogManager.GetLogger(typeof(LoggingBehaviorAttribute));

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
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, 
        /// message or parameter interceptors, security extensions, and other custom extension objects.
        /// Добавляет себя в коллекцию <see cref="ChannelDispatcher.ErrorHandlers"/> и в коллекцию <see cref="DispatchOperation.ParameterInspectors"/>.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                channelDispatcher.ErrorHandlers.Add(this);
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

        #region IErrorHandler

        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an exception in the course of a service method.
        /// </summary>
        /// <param name="error">
        /// The <see cref="T:System.Exception"/> object thrown in the course of the service operation.
        /// </param>
        /// <param name="version">
        /// The SOAP version of the message.
        /// </param>
        /// <param name="fault">
        /// The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or service, in the duplex case.
        /// </param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and the instance context in certain cases. 
        /// Логгирует ошибку.
        /// </summary>
        /// <returns>
        /// true if Windows Communication Foundation (WCF) should not abort the session (if there is one) and 
        /// instance context if the instance context is not <see cref="F:System.ServiceModel.InstanceContextMode.Single"/>; 
        /// otherwise, false. The default is false.
        /// </returns>
        /// <param name="error">
        /// The exception thrown during processing.
        /// </param>
        public bool HandleError(Exception error)
        {
            log.Error("Не обработанная ошибка", error);
            return false;
        }
        #endregion

        #region IParameterInspector

        /// <summary>
        /// Called before client calls are sent and after service responses are returned.
        /// Создает объект идентифкации запроса и запускает измерение выполнения метода.
        /// </summary>
        /// <returns>
        /// Возвращает объект идентификации запроса.
        /// </returns>
        /// <param name="operationName">
        /// The name of the operation.
        /// </param>
        /// <param name="inputs">
        /// The objects passed to the method by the client.
        /// </param>
        public object BeforeCall(string operationName, object[] inputs)
        {
            var requestMeasuring = new RequestMeasuring();
            var @params = string.Join(", ", inputs.Select(x => Serializer.Serialize(x)));
            log.InfoFormat(
                "Вызван метод {0} с параметрами: \"{1}\"; requestId = {2}",
                operationName,
                @params,
                requestMeasuring.RequestId);

            return requestMeasuring;
        }

        /// <summary>
        /// Called after client calls are returned and before service responses are sent.
        /// </summary>
        /// <param name="operationName">
        /// The name of the invoked operation.
        /// </param>
        /// <param name="outputs">
        /// Any output objects.
        /// </param>
        /// <param name="returnValue">
        /// The return value of the operation.
        /// </param>
        /// <param name="correlationState">
        /// Объект идентификации запроса.
        /// </param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            var requestMeasuring = correlationState as RequestMeasuring;

            var serializedReturnValue = Serializer.Serialize(returnValue);

            if (requestMeasuring == null)
            {
                log.WarnFormat("Выполнен метод {0} с результатом {1}; requestId = unknow", operationName, serializedReturnValue);
            }
            else
            {
                requestMeasuring.StopWatch.Stop();
                log.InfoFormat(
                    "Выполнен метод {0} с результатом {1} за {3} mc; requestId = {2}",
                    operationName,
                    serializedReturnValue,
                    requestMeasuring.RequestId,
                    requestMeasuring.StopWatch.ElapsedMilliseconds);
            }
        }
        #endregion

        /// <summary>
        /// Объект идентификации запроса.
        /// </summary>
        internal class RequestMeasuring
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RequestMeasuring"/> class.
            /// </summary>
            public RequestMeasuring()
            {
                this.RequestId = Guid.NewGuid();
                this.StopWatch = Stopwatch.StartNew();
            }

            /// <summary>
            /// Идентифкатор запроса.
            /// </summary>
            public Guid RequestId { get; private set; }

            /// <summary>
            /// Измеритель затраченного времени.
            /// </summary>
            public Stopwatch StopWatch { get; private set; }
        }
    }
}