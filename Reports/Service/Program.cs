using Topshelf;
using log4net;
using log4net.Config;

namespace Rapidsoft.VTB24.Reports.Service
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        static void Main()
        {
            XmlConfigurator.Configure();

            Log.Info("\n======== STARTED ========\n");

            HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.SetDescription("Vtb24 Loyalty Statistics Service");
                    c.SetDisplayName("Vtb24 Statistics");
                    c.SetServiceName("Vtb24Statistics");
                    c.RunAsLocalSystem();

                    c.Service<ServiceControl>(sc =>
                        {
                            sc.ConstructUsing(name => new ServiceControl());
                            sc.WhenStarted(s => s.Start());
                            sc.WhenStopped(s => s.Stop());
                        });
                });
        }
    }
}
