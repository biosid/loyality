using System;
using System.Collections.Generic;
using System.IO;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Console
{
    public sealed class ConsoleEtlLogger : IEtlLogger
    {
        #region Constructors

        public ConsoleEtlLogger(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            _writer = writer;
        }

        #endregion

        #region Fields

        private readonly TextWriter _writer;

        #endregion

        #region IEtlLogger Members

        public void LogEtlSessionStart(EtlSession session)
		{
        }

        public void LogEtlSessionContinue(EtlSession session)
        {
        }

        public void LogEtlSessionEnd(EtlSession session)
        {
        }

        public void LogEtlVariable(EtlVariable variable)
        {
        }

        public void LogEtlCounter(EtlCounter counter)
        {
        }

        public void LogEtlMessage(EtlMessage message)
        {
            _writer.WriteLine(string.Format("[{0}] {1}", GetNowString(), message.Text));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Methods

        private static string GetNowString()
        {
            return DateTime.Now.ToString("s");
        }

        #endregion
    }
}