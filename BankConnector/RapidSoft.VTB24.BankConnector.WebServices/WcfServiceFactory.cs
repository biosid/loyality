using Microsoft.Practices.Unity;
using RapidSoft.Etl.Logging;
using Unity.Wcf;

namespace RapidSoft.VTB24.BankConnector.WebServices
{
    using System.Configuration;

    using Microsoft.Practices.Unity.Configuration;

    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            container.LoadConfiguration(section);

            container.RegisterInstance(new EtlLogger.EtlLogger(new NullEtlLogger(), "notset", "notset"));
        }
    }
}