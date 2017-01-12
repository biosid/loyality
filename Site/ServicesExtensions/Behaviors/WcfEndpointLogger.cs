using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using Vtb24.Logging;
using log4net;

namespace Vtb24.ServicesExtensions.Behaviors
{
    public class WcfEndpointLogger : IDispatchMessageInspector, IClientMessageInspector, IEndpointBehavior
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (WcfEndpointLogger));

        public string LogMessageHeader { get; set; }

        #region IDispatchMessageInspector

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return LogRequest(request);
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            LogReply(reply, correlationState as Guid?);
        }

        #endregion

        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return LogRequest(request);
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            LogReply(reply, correlationState as Guid?);
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
            var logger = new WcfEndpointLogger
            {
                LogMessageHeader = CreateLogMessageHeader(endpoint, endpointDispatcher)
            };

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(logger);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var logger = new WcfEndpointLogger
            {
                LogMessageHeader = CreateLogMessageHeader(endpoint, clientRuntime)
            };

            clientRuntime.ClientMessageInspectors.Add(logger);
        }

        #endregion

        #region приватные методы

        public Guid? LogRequest(Message request)
        {
            var seqNum = Guid.NewGuid();

            _log.Info("\r\nBEGIN REQUEST #" + seqNum.ToString("d") +
                      "\r\n   HTTP Request ID: " + HttpHelpers.GetRequestId() +
                      "\r\nUser identity name: " + Thread.CurrentPrincipal.Identity.Name +
                      LogMessageHeader +
                      "\r\n           Request:\r\n" + request +
                      "\r\nEND REQUEST\r\n\r\n");

            return seqNum;
        }

        public void LogReply(Message reply, Guid? seqNum)
        {
            _log.Info("\r\nBEGIN REPLY #" + (seqNum.HasValue ? seqNum.Value.ToString("d") : "N/A") +
                      "\r\n   HTTP Request ID: " + HttpHelpers.GetRequestId() +
                      "\r\nUser identity name: " + Thread.CurrentPrincipal.Identity.Name +
                      LogMessageHeader +
                      "\r\n             Reply:\r\n" + reply +
                      "\r\nEND REPLY\r\n\r\n");
        }
        
        private static string CreateLogMessageHeader(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            return
                "\r\n           Address: " + endpoint.Address +
                "\r\n     Contract name: " + endpointDispatcher.ContractName;
        }

        private static string CreateLogMessageHeader(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            return
                "\r\n           Address: " + endpoint.Address +
                "\r\n     Contract name: " + clientRuntime.ContractName;
        }

        #endregion
    }
}
