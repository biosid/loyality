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
    using System.Linq;

    [Serializable]
    public sealed class EtlValidateCsvFileHeaderStep : EtlStep
    {
        #region Constructors

        public EtlValidateCsvFileHeaderStep()
        {
        }

        #endregion

        #region Constants

        public static readonly char DefaultQuote = '"';
        public static readonly char DefaultEscape = '"';
        public static readonly string DefaultLineDelimiter = "\r\n";

        #endregion

        #region Properties

        [Category("1. Source")]
        public EtlCsvFileInfo Source
        {
            get;
            set;
        }

        [Category("2. HeaderColumns")]
        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        [XmlArrayItem("HeaderColumn")]
        public List<CsvHeaderColumn> HeaderColumns
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

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            CheckArguments(context, logger);

            var csvSyntax = new CsvSyntaxInfo
            {
                HasHeaders = this.Source.HasHeaders,
                FieldDelimiter = this.Source.FieldDelimiter[0],
                Quote = this.Source.Quote != null ? this.Source.Quote[0] : DefaultQuote,
                Escape = this.Source.Escape != null ? this.Source.Escape[0] : DefaultEscape,
                LineDelimiter1 = this.Source.LineDelimiter != null ? this.Source.LineDelimiter[0] : DefaultLineDelimiter[0],
                LineDelimiter2 = this.Source.LineDelimiter != null ? this.Source.LineDelimiter[1] : DefaultLineDelimiter[1],
            };

            var erros = ValidateSource(csvSyntax);

            if (erros.Count > 0)
            {
                LogErrors(context, logger, erros);

                return new EtlStepResult(EtlStatus.Failed, null);
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        private void CheckArguments(EtlContext context, IEtlLogger logger)
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

            if (this.HeaderColumns == null)
            {
                this.HeaderColumns = new List<CsvHeaderColumn>();
            }
        }

        private void LogErrors(EtlContext context, IEtlLogger logger, List<string> erros)
        {
            foreach (var error in erros)
            {
                DateTime errorDateTime = DateTime.Now;

                logger.LogEtlMessage(
                    new EtlMessage
                    {
                        EtlPackageId = context.EtlPackageId,
                        EtlSessionId = context.EtlSessionId,
                        EtlStepName = this.Name,
                        LogDateTime = errorDateTime,
                        LogUtcDateTime = errorDateTime.ToUniversalTime(),
                        MessageType = EtlMessageType.Error,
                        Text = error,
                    });
            }
        }

        private List<string> ValidateSource(CsvSyntaxInfo csvSyntax)
        {
            var errors = new List<string>();

            BadFormatMessage = BadFormatMessage ?? @"Required column ""{0}"" in file ""{1}"" not found";

            using (var fileReader = new StreamReader(this.Source.FilePath, Encoding.GetEncoding(this.Source.CodePage)))
            {
                using (var csvReader = new CsvReader(fileReader, csvSyntax, this.Source.SkipEmptyRows))
                {
                    var headers = csvReader.GetFieldHeaders();

                    foreach (var headerColumn in HeaderColumns)
                    {
                        if (headers.All(h => h != headerColumn.Name))
                        {
                            errors.Add(string.Format(BadFormatMessage, headerColumn.Name, this.Source.FilePath));
                        }
                    }
                }
            }

            return errors;
        }

        #endregion
    }
}
