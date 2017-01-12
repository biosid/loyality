using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Globalization;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    internal sealed class CsvWriter
    {
        #region Constructors

        public CsvWriter(TextWriter writer, CsvSyntaxInfo syntax, int? batchSize)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (syntax == null)
            {
                throw new ArgumentNullException("syntax");
            }

            _writer = writer;

            _fieldDelimiter = syntax.FieldDelimiter.ToString();
            _lineDelimiter = string.Concat(syntax.LineDelimiter1, syntax.LineDelimiter2);
            _quote = syntax.Quote.ToString();
            _escape = syntax.Escape.ToString();
	        _batchSize = batchSize;

            _escapeAndQuote = string.Concat(_escape, _quote);
            _escapeAndEscape = string.Concat(_escape, _escape);

            _hasHeaders = syntax.HasHeaders;
        }

        #endregion

        #region Fields

        private readonly TextWriter _writer;

        private readonly bool _hasHeaders;
        private readonly string _lineDelimiter;
        private readonly string _fieldDelimiter;
        private readonly string _quote;
        private readonly string _escape;
        private readonly string _escapeAndQuote;
        private readonly string _escapeAndEscape;
	    private readonly int? _batchSize;

        private readonly IFormatProvider _formatProvider = CultureInfo.InvariantCulture;

        #endregion

        #region Methods

        /// <summary>
        /// Записывает данные в связанный файл
        /// </summary>
        /// <param name="reader">Источник данных</param>
        /// <param name="continueReading">Признак, что данные первой строки были прочитаны в reader и требуется их запись в файл, флаг также устанавливается, если текущий файл при пакетной записи заполнен, но есть незаписанные данные</param>
        /// <returns>Количество записей, которые были записаны</returns>
        public long Write(IDataReader reader, ref bool continueReading)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var count = 0L;
            var columnNames = GetDataReaderColumnNames(reader);
            if (_hasHeaders)
            {
                for (var i = 0; i < columnNames.Length; i++)
                {
                    if (i > 0)
                    {
                        _writer.Write(_fieldDelimiter);
                    }

                    WriteValue(columnNames[i]);
                }
            }

            // записывается первая строка с данными
            if (continueReading)
            {
                if (_hasHeaders)
                {
                    _writer.Write(_lineDelimiter);
                }
                WriteLine(reader);
                count++;
                continueReading = false;
            }

			if (_batchSize != null)
			{
                // пишется группа записей начиная со второй в текущем пакете
				while ((continueReading = reader.Read()) && count < _batchSize)
				{
					_writer.Write(_lineDelimiter);

					WriteLine(reader);
					count++;
				}
			}
			else
			{
				while (reader.Read())
				{
					_writer.Write(_lineDelimiter);

					WriteLine(reader);
					count++;
				}
			}


            return count;
        }

        private void WriteLine(IDataReader reader)
        {
            WriteValue(reader[0]);
            for (var i = 1; i < reader.FieldCount; i++)
            {
                _writer.Write(_fieldDelimiter);
                WriteValue(reader[i]);
            }
        }

        private void WriteValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                _writer.Write("");
                return;
            }

            //todo: create CsvStringConverter and use it or allow only strings
            var str = EtlValueConverter.ToString(value);
            var containsQuote = str.Contains(_quote);
            var containsEscape = str.Contains(_escape);
            var containsDelimiters =
                containsQuote ||
                containsEscape ||
                str.Contains(_fieldDelimiter) ||
                str.Contains(_lineDelimiter);

            if (containsEscape)
            {
                str = EscapeString(str);
            }

            if (containsDelimiters)
            {
                _writer.Write(_quote);
                _writer.Write(str);
                _writer.Write(_quote);
            }
            else
            {
                _writer.Write(str);
            }
        }

        private string EscapeString(string str)
        {
            return str
                //.Replace(_quote, _escapeAndQuote)
                .Replace(_escape, _escapeAndEscape);
        }

        private static string[] GetDataReaderColumnNames(IDataReader reader)
        {
            var names = new string[reader.FieldCount];

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                names[i] = name;
            }

            return names;
        }

        #endregion
    }
}
