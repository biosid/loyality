using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Quartz.Spi;
using Quartz.Unity;

namespace ScheduledJobs.App_Start
{
    public class UnityConfig
    {
        public static IUnityContainer CreateContiner()
        {
            var container = new UnityContainer();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(section);

            return container;
        }

        public static IJobFactory GetJobFactory(IUnityContainer unityContainer)
        {
            var factory = new UnityJobFactory(unityContainer);
            return factory;
        }
    }
}