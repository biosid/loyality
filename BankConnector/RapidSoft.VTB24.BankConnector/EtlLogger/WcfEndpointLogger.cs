namespace RapidSoft.VTB24.BankConnector.EtlLogger
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Threading;

    using RapidSoft.Loaylty.Logging;

    public class WcfEndpointLogger : IClientMessageInspector, IEndpointBehavior
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(WcfEndpointLogger));

        public string LogMessageHeader { get; set; }

        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var seqNum = (Guid?)Guid.NewGuid();

            logger.Info("\r\nBEGIN REQUEST #" + seqNum.Value.ToString("d") +
                      "\r\nUser identity name: " + Thread.CurrentPrincipal.Identity.Name +
                      this.LogMessageHeader +
                      "\r\n           Request:\r\n" + request +
                      "\r\nEND REQUEST\r\n\r\n");

            return seqNum;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var seqNum = correlationState as Guid?;

            logger.Info("\r\nBEGIN REPLY #" + (seqNum.HasValue ? seqNum.Value.ToString("d") : "N/A") +
                      "\r\nUser identity name: " + Thread.CurrentPrincipal.Identity.Name +
                      this.LogMessageHeader +
                      "\r\n             Reply:\r\n" + reply +
                      "\r\nEND REPLY\r\n\r\n");
        }

        #endregion

        #region IEndpointBehavior

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var endpointLogger = new WcfEndpointLogger
            {
                LogMessageHeader = CreateLogMessageHeader(endpoint, clientRuntime)
            };

            clientRuntime.ClientMessageInspectors.Add(endpointLogger);
        }

        #endregion

        private static string CreateLogMessageHeader(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            return
                "\r\n           Address: " + endpoint.Address +
                "\r\n     Contract name: " + clientRuntime.ContractName;
        }
    }
}
