namespace RapidSoft.VTB24.BankConnector.EtlLogger
{
    using System;
    using System.ServiceModel.Configuration;

    public class WcfEndpointLoggerExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(WcfEndpointLogger); }
        }

        protected override object CreateBehavior()
        {
            return new WcfEndpointLogger();
        }
    }
}