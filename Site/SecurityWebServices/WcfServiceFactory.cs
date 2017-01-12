using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Wcf;
using Vtb24.Site.SecurityWebServices.PublicProfile;
using Vtb24.Site.SecurityWebServices.Security;
using Vtb24.Site.SecurityWebServices.SecurityToken;

namespace Vtb24.Site.SecurityWebServices
{
	public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            var section = (UnityConfigurationSection) ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(section);
            container.RegisterType<ISecurityWebApi, SecurityWebApi>();
            container.RegisterType<IPublicProfileWebApi, PublicProfileWebApi>();
            container.RegisterType<ISecurityTokenWebApi, SecurityTokenWebApi>();
        }
    }    
}