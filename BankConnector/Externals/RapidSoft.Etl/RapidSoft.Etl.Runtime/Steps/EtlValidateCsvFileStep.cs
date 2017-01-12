using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text;
using System.IO;

using RapidSoft.Etl.Runtime.DataSources.Csv;
using RapidSoft.Etl.Runtime.Properties;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add statistics
    [Serializable]
    public sealed class EtlValidateCsvFileStep : EtlStep
    {
        #region Constructors

        public EtlValidateCsvFileStep()
        {
        }

        #endregion

        #region Constants

        public static readonly char DefaultQuote = '"';
        public static readonly char DefaultEscape = '"';
        public static readonly string DefaultLineDelimiter = "\r\n";

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlCsvFileInfo Source
        {
            get;
            set;
        }

        [Category("3. Validation")]
        public EtlValidationErrorBehavior ErrorBehavior
        {
            get;
            set;
        }

        [Category("3. Validation")]
        public string BadFormatMessage
        {
            get;
            set;
        }

        //[Category("3. Validation")]
        //[XmlArrayItem("HeaderValidationRule")]
        //public List<EtlFieldValidationRule> HeaderValidationRules
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return _headerValidationRules;
        //    }
        //}
        //private List<EtlFieldValidationRule> _headerValidationRules = new List<EtlFieldValidationRule>();

        [Category("3. Validation")]
        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        [XmlArrayItem("FieldValidationRule")]
        public List<EtlFieldValidationRule> FieldValidationRules
        {
            [DebuggerStepThrough]
            get
            {
                return _fieldValidationRules;
            }
        }
        private List<EtlFieldValidationRule> _fieldValidationRules = new List<EtlFieldValidationRule>();

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (this.Source == null)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Source"));
            }

            if (string.IsNullOrEmpty(this.Source.FieldDelimiter))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Source.FieldDelimiter"));
            }

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Source.FilePath"));
            }

            if (this.Source.FieldDelimiter.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyTooLong, "Source.FieldDelimiter", 1));
            }

            if (this.Source.Quote != null && this.Source.Quote.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyTooLong, "Source.Quote", 1));
            }

            if (this.Source.Escape != null && this.Source.Escape.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyTooLong, "Source.Escape", 1));
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);

            var sourceRowCount = 0L;
            var validRowCount = 0L;
            var errorRowCount = 0L;

            var csvSyntax = new CsvSyntaxInfo
            {
                HasHeaders = this.Source.HasHeaders,
                FieldDelimiter = this.Source.FieldDelimiter[0],
                Quote = this.Source.Quote != null ? this.Source.Quote[0] : DefaultQuote,
                Escape = this.Source.Escape != null ? this.Source.Escape[0] : DefaultEscape,
                LineDelimiter1 = this.Source.LineDelimiter != null ? this.Source.LineDelimiter[0] : DefaultLineDelimiter[0],
                LineDelimiter2 = this.Source.LineDelimiter != null ? this.Source.LineDelimiter[1] : DefaultLineDelimiter[1],
            };

            using (var fileReader = new StreamReader(this.Source.FilePath, Encoding.GetEncoding(this.Source.CodePage)))
            {
                using (var csvReader = new CsvReader(fileReader, csvSyntax, this.Source.SkipEmptyRows))
                {
                    var validator = new CsvValidator(this, csvReader, context, logger);
                    result.Status = validator.Validate();
                    sourceRowCount = validator.ReadRowCount;
                    validRowCount = validator.ValidRowCount;
                    errorRowCount = validator.ErrorRowCount;
                }
            }

            //logger.LogEtlMessage(new EtlMessage
            //{
            //    EtlPackageId = context.EtlPackageId,
            //    EtlSessionId = context.EtlSessionId,
            //    EtlStepId = this.Id,
            //    LogDateTime = endDateTime,
            //    LogUtcDateTime = endDateTime.ToUniversalTime(),
            //    MessageType = EtlMessageType.Statistics,
            //    Text = "Found", 
            //    Flags = sourceRowCount,
            //});

            //logger.LogEtlMessage(new EtlMessage
            //{
            //    EtlPackageId = context.EtlPackageId,
            //    EtlSessionId = context.EtlSessionId,
            //    EtlStepId = this.Id,
            //    LogDateTime = endDateTime,
            //    LogUtcDateTime = endDateTime.ToUniversalTime(),
            //    MessageType = EtlMessageType.Statistics,
            //    Text = "Validated", 
            //    Flags = validRowCount,
            //});

            //logger.LogEtlMessage(new EtlMessage
            //{
            //    EtlPackageId = context.EtlPackageId,
            //    EtlSessionId = context.EtlSessionId,
            //    EtlStepId = this.Id,
            //    LogDateTime = endDateTime,
            //    LogUtcDateTime = endDateTime.ToUniversalTime(),
            //    MessageType = EtlMessageType.Statistics,
            //    Text = "Errors", 
            //    Flags = errorRowCount,
            //});

            return result;
        }

        #endregion

        #region Nested classes

        private sealed class CsvValidator
        {
            #region Constructors

            public CsvValidator(EtlValidateCsvFileStep step, CsvReader reader, EtlContext context, IEtlLogger logger)
            {
                _step = step;
                _reader = reader;
                _context = context;
                _logger = logger;

                _reader.ParseError += OnReaderError;
            }

            #endregion

            #region Fields

            private readonly EtlValidateCsvFileStep _step;
            private readonly EtlContext _context;
            private readonly CsvReader _reader;
            private readonly IEtlLogger _logger;

            private bool _wasReaderError;

            private long _readRowCount;
            private long _validRowCount;
            private long _errorRowCount;

            #endregion

            #region Properties

            public long ReadRowCount
            {
                [DebuggerStepThrough]
                get
                {
                    return _readRowCount;
                }
            }

            public long ValidRowCount
            {
                [DebuggerStepThrough]
                get
                {
                    return _validRowCount;
                }
            }

            public long ErrorRowCount
            {
                [DebuggerStepThrough]
                get
                {
                    return _errorRowCount;
                }
            }

            #endregion

            #region Methods

            public EtlStatus Validate()
            {
                _readRowCount = 0;

                if (_step.Source.HasHeaders)
                {
                    var isValid = ValidateHeaders();
                    if (!isValid)
                    {
                        return EtlStatus.Failed;
                    }
                }

                var wasErrors = false;

                while (_reader.Read())
                {
                    if (_wasReaderError)
                    {
                        return EtlStatus.Failed;
                    }

                    _readRowCount++;

                    wasErrors = !ValidateRow();
                    if (wasErrors)
                    {
                        _errorRowCount++;
                        if (_step.ErrorBehavior == EtlValidationErrorBehavior.ValidateUntilError)
                        {
                            return EtlStatus.Failed;
                        }
                    }
                    else
                    {
                        _validRowCount++;
                    }
                }

                return wasErrors ? EtlStatus.Failed : EtlStatus.Succeeded;
            }

            private void OnReaderError(object sender, CsvParseErrorEventArgs e)
            {
                _wasReaderError = true;

                LogValidationErrorMessage
                (
                    _step.BadFormatMessage,
                    e.Error.CurrentRecordIndex,
                    e.Error.CurrentFieldIndex,
                    null,
                    e.Error.CurrentPosition
                );

                e.Action = CsvParseErrorAction.AdvanceToNextLine;
            }

            private bool ValidateHeaders()
            {
                var headers = _reader.GetFieldHeaders();
                if (_wasReaderError)
                {
                    return false;
                }

                var wasErrors = false;
                return !wasErrors;
            }

            private int GetHeaderIndexByName(string[] headers, string header)
            {
                for (var i = 0; i < headers.Length; i++)
                {
                    if (string.Equals(headers[i], header, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return i;
                    }
                }

                return -1;
            }

            private bool ValidateRow()
            {
                var wasErrors = false;

                foreach (var rule in _step.FieldValidationRules)
                {
                    if (!ValidateRule(rule))
                    {
                        wasErrors = true;
                    }
                }

                return !wasErrors;
            }

            private bool ValidateRule(EtlFieldValidationRule rule)
            {
                var value = ReadValue(rule);
                if (_wasReaderError)
                {
                    return false;
                }

                if (!rule.IsValid(value))
                {
                    LogValidationErrorMessage
                    (
                        rule.ErrorMessage,
                        _reader.CurrentRecordIndex,
                        null,
                        rule.SourceName,
                        null
                    );

                    return false;
                }
                else
                {
                    return true;
                }
            }

            private void LogValidationErrorMessage
            (
                string errorMessage,
                long errorRecordIndex,
                int? errorFieldIndex,
                string errorFieldName,
                long? errorFilePosition
            )
            {
                var errorDateTime = DateTime.Now;

                _logger.LogEtlMessage
                (
                    new EtlMessage
                    {
                        EtlPackageId = _context.EtlPackageId,
                        EtlSessionId = _context.EtlSessionId,
                        EtlStepName = _step.Name,
                        LogDateTime = errorDateTime,
                        LogUtcDateTime = errorDateTime.ToUniversalTime(),
                        MessageType = EtlMessageType.Error,
                        Text = errorMessage,
                        Flags = errorRecordIndex,
                        StackTrace = BuildErrorMessageStackTrace
                        (
                            errorRecordIndex,
                            errorFieldIndex,
                            errorFieldName,
                            errorFilePosition
                        ),
                    }
                );

            }

            private string BuildErrorMessageStackTrace
            (
                long errorRecordIndex,
                int? errorFieldIndex,
                string errorFieldName,
                long? errorFilePosition
            )
            {
                var sb = new StringBuilder();

                var outputRecordIndex = _step.Source.HasHeaders ? errorRecordIndex + 2 : errorRecordIndex + 1;
                sb.Append(Resources.ValidateCsvErrorLinePrefix);
                sb.Append(" ");
                sb.Append(outputRecordIndex);


                if (errorFieldIndex.HasValue)
                {
                    var outputFieldIndex = errorFieldIndex.HasValue ? (errorFieldIndex + 1).ToString() : errorFieldName;
                    sb.Append(", ");
                    sb.Append(Resources.ValidateCsvErrorColumnPrefix);
                    sb.Append(" ");
                    sb.Append(outputFieldIndex);
                }

                if (errorFilePosition.HasValue)
                {
                    sb.Append(", ");
                    sb.Append(Resources.ValidateCsvErrorPositionPrefix);
                    sb.Append(" ");
                    sb.Append(errorFilePosition.Value);
                }

                return sb.ToString();
            }

            private string ReadValue(EtlFieldValidationRule rule)
            {
                if (!string.IsNullOrEmpty(rule.SourceName))
                {
                    return _reader[rule.SourceName];
                }
                else
                {
                    return null;
                }
            }

            #endregion
        }

        #endregion
    }
}
