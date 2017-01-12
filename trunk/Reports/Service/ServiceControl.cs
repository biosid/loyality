using System.Configuration;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Rapidsoft.VTB24.Reports.IisLogs;
using Rapidsoft.VTB24.Reports.Statistics;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.Models.ProductViewEvents;
using Rapidsoft.VTB24.Reports.Statistics.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.ProductViewEvents;
using Rapidsoft.VTB24.Reports.Statistics.Workers;
using log4net;

namespace Rapidsoft.VTB24.Reports.Service
{
    class ServiceControl
    {
        private class WorkerContext
        {
            public IWorker Worker { get; private set; }

            public Thread Thread { get; private set; }

            public WorkerContext(IWorker worker)
            {
                Worker = worker;
                Thread = new Thread(worker.Execute);
            }
        }

        private readonly ILog _log = LogManager.GetLogger(typeof (ServiceControl));

        private readonly WorkerContext[] _workers;

        public ServiceControl()
        {
            var container = new UnityContainer();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(section);

            var iisLogsScanner = container.Resolve<IIisLogsScanner>();

            _workers = new[]
            {
                new WorkerContext(new IisLogsWorker<ReportItem>(new PixelReportsJobs(iisLogsScanner))),
                new WorkerContext(new IisLogsWorker<ViewEvent>(new ProductViewEventsJobs(iisLogsScanner)))
            };
        }

        public void Start()
        {
            foreach (var worker in _workers)
            {
                worker.Thread.Start();
            }
        }

        public void Stop()
        {
            foreach (var worker in _workers)
            {
                worker.Worker.Stop();
            }

            foreach (var worker in _workers)
            {
                if (!worker.Thread.Join(10000))
                {
                    _log.Error("не удалось дождаться остановки рабочего потока: " + worker.Worker.GetType().FullName);
                }
            }
        }
    }
}
