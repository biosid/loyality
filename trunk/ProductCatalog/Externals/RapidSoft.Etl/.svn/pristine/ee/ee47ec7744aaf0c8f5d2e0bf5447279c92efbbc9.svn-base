using System;
using System.Collections.Generic;
using System.IO;

namespace RapidSoft.Etl.Logging
{
    public sealed class CsvEtlLogger : IEtlLogger
    {
        #region Constructors

        public CsvEtlLogger(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            _writer = writer;
        }

        #endregion

        #region Constants

        private const string QUOTE = "\"";
        private const string ESCAPE = "\\";
        private const string ESCAPE_AND_ESCAPE = "\\\\";
        private const string ESCAPE_AND_QUOTE = "\\\"";
        private const string FIELD_DELIMITER = ";";
        private const string LINE_DELIMITER = "\r\n";

        #endregion

        #region Fields

        private readonly TextWriter _writer;

        private static readonly string MessageFormat = string.Concat(
            "{0}", FIELD_DELIMITER,
            "{1}", FIELD_DELIMITER,
            "{2}", FIELD_DELIMITER,
            "{3}", FIELD_DELIMITER,
            "{4}", FIELD_DELIMITER, 
            "{5}", FIELD_DELIMITER, 
            "{6}", FIELD_DELIMITER,
            "{7}", FIELD_DELIMITER, 
            "{8}"
        );

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

        public void LogEtlCounter(EtlCounter counters)
        {
        }

        public void LogEtlMessage(EtlMessage message)
        {
            _writer.Write(MessageFormat,
                FormatDateTime(message.LogDateTime), //0
                FormatString(message.EtlPackageId), //1
                FormatString(message.EtlSessionId), //2
                FormatString(message.EtlStepName), //3
                message.MessageType, //4
                FormatString(message.Text), //5
                FormatString(message.StackTrace), //6
                message.Flags, //7
                FormatDateTime(message.LogUtcDateTime) //8
            );
            _writer.Write(LINE_DELIMITER);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _writer.Dispose();
        }

        #endregion

        #region Methods

        public void WriteHeaders()
        {
            _writer.WriteLine(MessageFormat,
                "LogDateTime", //0
                "EtlPackageId", //1
                "EtlSessionId", //2
                "EtlStepName", //3
                "MessageType", //4
                "message.Text", //5
                "RowCount", //6
                "RowNumber", //7
                "DurationMilliseconds", //8
                "StackTrace", //9
                "LogUtcDateTime" //10
            );
        }

        private string FormatString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return string.Concat(
                    QUOTE, 
                    str
                        .Replace(QUOTE, ESCAPE_AND_QUOTE)
                        .Replace(ESCAPE, ESCAPE_AND_ESCAPE),
                    QUOTE
                );
            }
        }

        private static string FormatDateTime(DateTime dt)
        {
            return dt.ToString("s");
        }

        #endregion
    }
}