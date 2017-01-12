using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Vtb24.ServicesExtensions.Behaviors
{
    public class SerilogEndpointBehaviour : IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var inspector = new SerilogMessageInspector(endpoint, endpointDispatcher);

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new SerilogMessageInspector(endpoint, clientRuntime);

            clientRuntime.ClientMessageInspectors.Add(inspector);
        }
    }
}
