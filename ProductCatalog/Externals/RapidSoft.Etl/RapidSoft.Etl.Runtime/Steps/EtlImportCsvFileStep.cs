using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Data;
using System.IO;
using System.Text;

using RapidSoft.Etl.Runtime.DataSources.Csv;
using RapidSoft.Etl.Runtime.DataSources.DB;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add statistics
    [Serializable]
    public sealed class EtlImportCsvFileStep : EtlStep
    {
        #region Constructors

        public EtlImportCsvFileStep()
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

        [Category("3. Destination")]
        public EtlTableInfo Destination
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public int BatchSize
        {
            get;
            set;
        }

        [Category("4. Mappings")]
        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        [XmlArrayItem("Mapping")]
        public List<EtlFieldMapping> Mappings
        {
            [DebuggerStepThrough]
            get
            {
                return _mappings;
            }
        }
        private List<EtlFieldMapping> _mappings = new List<EtlFieldMapping>();

        [Category("5. Errors")]
        public EtlImportDataLossBehavior DataLossBehavior
        {
            get;
            set;
        }

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

            if (this.Destination == null)
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Destination"));
            }

            if (string.IsNullOrEmpty(this.Source.FieldDelimiter))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Source.FieldDelimiter"));
            }

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Source.FilePath"));
            }

            if (string.IsNullOrEmpty(this.Destination.ConnectionString))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Destination.ConnectionString"));
            }

            if (string.IsNullOrEmpty(this.Destination.ProviderName))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Destination.ProviderName"));
            }

            if (string.IsNullOrEmpty(this.Destination.TableName))
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.PropertyCannotBeNull, "Destination.TableName"));
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
            var insertedRowCount = 0L;
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
                using (var csvReader = new CsvReader(fileReader, csvSyntax, this.Source.SkipEmptyRows, this.Source.NullForSkipColumn))
                {
                    var wrappedReader = new EtlMappedDataReader(csvReader, this.Mappings);

                    var dbWriter = new DBTableWriter(this.Destination.ConnectionString, this.Destination.ProviderName, this.Destination.TableName);
                    dbWriter.ErrorOccured += delegate(object sender, DBTableWriterErrorEventArgs e)
                    {
                        var errorDateTime = DateTime.Now;
                        result.Status = EtlStatus.FinishedWithLosses;
                        result.Message = e.Message;

                        //todo: fix error counter in CSV import. Now it counts errors, not error records
                        errorRowCount++;

                        logger.LogEtlMessage
                        (
                            new EtlMessage
                            {
                                EtlPackageId = context.EtlPackageId,
                                EtlSessionId = context.EtlSessionId,
                                EtlStepName = this.Name,
                                LogDateTime = errorDateTime,
                                LogUtcDateTime = errorDateTime.ToUniversalTime(),
                                MessageType = EtlMessageType.Error,
                                Text = e.Message,
                                Flags = e.RecordIndex,
                                StackTrace = e.Exception != null ? e.Exception.StackTrace : null,
                            }
                        );

                        if (this.DataLossBehavior == EtlImportDataLossBehavior.Skip)
                        {
                            result.Status = EtlStatus.FinishedWithLosses;
                            e.TrySkipError = true;
                        }
                        else
                        {
                            result.Status = EtlStatus.Failed;
                        }
                    };

                    insertedRowCount = dbWriter.Write(wrappedReader, this.TimeoutMilliseconds, this.BatchSize);
                    sourceRowCount = csvReader.CurrentRecordIndex + 1;
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
            //    Text = "Errors",
            //    Flags = errorRowCount,
            //});

            //logger.LogEtlMessage(new EtlMessage
            //{
            //    EtlPackageId = context.EtlPackageId,
            //    EtlSessionId = context.EtlSessionId,
            //    EtlStepId = this.Id,
            //    LogDateTime = endDateTime,
            //    LogUtcDateTime = endDateTime.ToUniversalTime(),
            //    MessageType = EtlMessageType.Statistics,
            //    Text = "Inserted",
            //    Flags = insertedRowCount,
            //});

            return result;
        }

        #endregion
    }
}
