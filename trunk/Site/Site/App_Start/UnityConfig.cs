using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.Mvc3;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Infrastructure.Caching;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;

namespace Vtb24.Site.App_Start
{
    public static class UnityConfig
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();
            container.AddNewExtension<Interception>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            var section = (UnityConfigurationSection) ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(section);

            container.RegisterType<HttpContextBase>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current))
            );

            container.RegisterType<ClientPrincipal>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<ISecurityService>().GetPrincipal())
            );

            container.RegisterType<ICacheCleaner, CacheCleaner>(
                new ContainerControlledLifetimeManager()
            );

            // ресолвер для полей аттрибутов-фильтров
            RemoveFilterAttributeFilterProvider();
            var filterAttributeFIlterProvider = new UnityFilterAttributeFilterProvider(container);
            container.RegisterInstance<IFilterProvider>(filterAttributeFIlterProvider);

            return container;
        }

        private static void RemoveFilterAttributeFilterProvider()
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
        }
    }
}