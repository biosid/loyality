using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using log4net;

namespace Rapidsoft.VTB24.Reports.Statistics.Workers
{
    public class IisLogsWorker<TItem> : IWorker
        where TItem : class
    {
        public IisLogsWorker(IIisLogsJobs<TItem> jobs)
        {
            _buffer = new List<TItem>(_bufferSize);
            _jobs = jobs;
        }

        private const int BATCH_SIZE = 100;

        private readonly ILog _log = LogManager.GetLogger(typeof(IisLogsWorker<TItem>));
        private readonly int _bufferSize = int.Parse(ConfigurationManager.AppSettings["vtb24:stat_service:items_buffer_size"]);
        private readonly List<TItem> _buffer;
        private readonly IIisLogsJobs<TItem> _jobs;

        private readonly int _pauseInterval = 1000 * int.Parse(ConfigurationManager.AppSettings["vtb24:stat_service:pause_interval_seconds"]);
        private readonly AutoResetEvent _resumeEvent = new AutoResetEvent(false);
        private volatile bool _stopRequested;

        public void Stop()
        {
            _log.Info("поступил запрос на остановку потока");
            _stopRequested = true;
            _resumeEvent.Set();
        }

        public void Execute()
        {
            _log.Info("поток запущен");

            while (!_stopRequested)
            {
                try
                {
                    _jobs.PeekJob();

                    if (_jobs.CurrentJobItems == null)
                    {
                        _log.InfoFormat("новых запросов нет, поток приостановлен на {0} миллисекунд", _pauseInterval);
                        Pause();
                    }
                    else
                    {
                        _log.Info("начата обработка запроса");

                        ExecuteJob();

                        _log.Info("закончена обработка запроса");
                    }
                }
                catch (Exception e)
                {
                    _log.Error("ошибка потока", e);
                    _log.InfoFormat("поток приостановлен на {0} миллисекунд", _pauseInterval);
                    Pause();
                }
            }

            _log.Info("поток остановлен");
        }

        private void Pause()
        {
            _resumeEvent.WaitOne(_pauseInterval);
        }

        private void ExecuteJob()
        {
            try
            {
                _buffer.Clear();

                _jobs.NotifyJobStarted();

                foreach (var item in _jobs.CurrentJobItems)
                {
                    if (_stopRequested)
                    {
                        _jobs.NotifyJobCancelled();
                        break;
                    }

                    if (item != null)
                    {
                        AppendItem(item);
                    }
                }

                if (_buffer.Count > 0)
                {
                    FlushBuffer();
                }

                _jobs.NotifyJobSucceeded();
            }
            catch (Exception e)
            {
                _log.Error("ошибка при генерации отчета", e);
                _jobs.NotifyJobFailed();
            }
        }

        private void AppendItem(TItem item)
        {
            _buffer.Add(item);

            if (_buffer.Count >= BATCH_SIZE)
            {
                FlushBuffer();
            }
        }

        private void FlushBuffer()
        {
            _jobs.SaveBatch(_buffer.ToArray());
            _buffer.Clear();
        }
    }
}
