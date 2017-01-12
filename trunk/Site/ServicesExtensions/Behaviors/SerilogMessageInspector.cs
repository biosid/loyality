using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Vtb24.ServicesExtensions.ServiceLogger;

namespace Vtb24.ServicesExtensions.Behaviors
{
    public class SerilogMessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        public SerilogMessageInspector(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            _log = new SerilogServiceLogger(endpointDispatcher.DispatchRuntime.Type.FullName,
                                            ServiceLogSide.Service,
                                            new { EndpointAddress = endpoint.Address.ToString() });
        }

        public SerilogMessageInspector(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            _log = new SerilogServiceLogger(clientRuntime.ContractClientType.FullName,
                                            ServiceLogSide.Client,
                                            new { EndpointAddress = endpoint.Address.ToString() });
        }

        private readonly SerilogServiceLogger _log;

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

        #region приватные методы

        private Guid? LogRequest(Message request)
        {
            var requestId = Guid.NewGuid();

            _log.Request(request, requestId);

            return requestId;
        }

        private void LogReply(Message reply, Guid? requestId)
        {
            _log.Reply(reply, requestId);
        }

        #endregion
    }
}
