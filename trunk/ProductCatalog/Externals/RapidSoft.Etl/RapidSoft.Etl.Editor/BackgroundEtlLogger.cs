using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Editor
{
    internal sealed class BackgroundEtlLogger : IEtlLogger
    {
        #region Constructors

        public BackgroundEtlLogger(BackgroundWorker worker, int packageStepCount)
        {
            if (worker == null)
            {
                throw new ArgumentNullException("worker");
            }
                        
            _worker = worker;
            _packageStepCount = packageStepCount;
            _currentPackageStepIndex = 0;
        }

        #endregion

        #region Fields

        private readonly BackgroundWorker _worker;
        private readonly int _packageStepCount;

        private int _currentPackageStepIndex;
        private int _currentProgress;

        #endregion

        #region IEtlLogger Members

        public void LogEtlSessionStart(EtlSession session)
		{
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _currentProgress = 0;
            //_worker.ReportProgress(_currentProgress, null);
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
        }

        public void LogEtlSessionEnd(EtlSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _currentProgress = 100;
            //_worker.ReportProgress(_currentProgress, null);
        }

        public void LogEtlVariable(EtlVariable variable)
        {
        }

        public void LogEtlCounter(EtlCounter counter)
        {
        }

        public void LogEtlMessage(EtlMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.MessageType == EtlMessageType.StepStart)
            {
                _currentPackageStepIndex++;
                _currentProgress = ((_currentPackageStepIndex - 1) * 100) / _packageStepCount;
            }

            //_worker.ReportProgress(_currentProgress, message);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}