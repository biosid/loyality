using System;
using System.ServiceModel.Configuration;

namespace Vtb24.ServicesExtensions.Behaviors
{
    public class SerilogEndpointBehaviourExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new SerilogEndpointBehaviour();
        }

        public override Type BehaviorType
        {
            get { return typeof (SerilogEndpointBehaviour); }
        }
    }
}
