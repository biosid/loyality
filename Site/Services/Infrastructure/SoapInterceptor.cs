using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Vtb24.Site.Services.Infrastructure
{
    internal class SoapInterceptor : IEndpointBehavior
    {
        private readonly NameValueCollection _log;


        public SoapInterceptor(NameValueCollection dataHolder)
        {
            _log = dataHolder;
        }


        public void Validate(ServiceEndpoint endpoint) { }


        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }


        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }


        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector(_log));
        }


        private class MessageInspector : IClientMessageInspector
        {
            private readonly NameValueCollection _log;


            public MessageInspector(NameValueCollection dataHolder)
            {
                _log = dataHolder;
            }


            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
                _log["response"] = reply.ToString();
            }


            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                _log["request"] = request.ToString();
                return null;
            }
        }
    }

}