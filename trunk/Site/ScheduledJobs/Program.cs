using ScheduledJobs.App_Start;
using Serilog.Extras.Topshelf;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;
using Vtb24.Logging;

namespace ScheduledJobs
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                SetupHost(c);
                SetupQuartz(c);
                c.Service<Service>(s =>
                {
                    SetupService(s);
                    ScheduleJobs(s);
                });
            });
        }

        private static void SetupHost(HostConfigurator configurator)
        {
            configurator.UseSerilog(SerilogLoggers.MainLogger.ForContext<Program>());
            configurator.SetDescription("Quartz Server: задачи фронтенда");
            configurator.SetDisplayName("Quartz Server");
            configurator.SetServiceName("QuartzServer");
            configurator.RunAsLocalSystem();
        }

        private static void SetupQuartz(HostConfigurator configurator)
        {
            var container = UnityConfig.CreateContiner();
            var factory = UnityConfig.GetJobFactory(container);
            configurator.UsingQuartzJobFactory(() => factory);
        }

        private static void SetupService(ServiceConfigurator<Service> configurator)
        {
            configurator.ConstructUsing(name => new Service());
            configurator.WhenStarted((service, control) => service.Start());
            configurator.WhenStopped((service, control) => service.Stop());
        }

        private static void ScheduleJobs(ServiceConfigurator<Service> configurator)
        {
            foreach (var job in JobsConfig.ListJobs())
            {
                configurator.ScheduleQuartzJob(job);
            }
        }
    }
}
