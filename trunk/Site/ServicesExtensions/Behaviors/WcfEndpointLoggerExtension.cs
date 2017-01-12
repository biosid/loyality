using System;
using System.ServiceModel.Configuration;

namespace Vtb24.ServicesExtensions.Behaviors
{
    public class WcfEndpointLoggerExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new WcfEndpointLogger();
        }

        public override Type BehaviorType
        {
            get { return typeof(WcfEndpointLogger); }
        }
    }
}
