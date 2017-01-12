namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.DataSources.Csv;
    using RapidSoft.Etl.Runtime.DataSources.DB;
    using RapidSoft.Etl.Runtime.Properties;

    [Serializable]
    public sealed class EtlImportCsvFileBatchStep : EtlStep
    {
        #region Constants

        public static readonly char DefaultQuote = '"';

        public static readonly char DefaultEscape = '"';

        public static readonly string DefaultLineDelimiter = "\r\n";

        #endregion

        #region Properties

        private List<EtlFieldMapping> _mappings = new List<EtlFieldMapping>();

        [Category("2. Source")]
        public EtlCsvFileBatchInfo Source
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
                return this._mappings;
            }
        }

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
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeNull, "Source"));
            }

            if (this.Destination == null)
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeNull, "Destination"));
            }

            if (string.IsNullOrEmpty(this.Source.FieldDelimiter))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Source.FieldDelimiter"));
            }

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeNull, "Source.FilePath"));
            }

            if (string.IsNullOrEmpty(this.Destination.ConnectionString))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.ConnectionString"));
            }

            if (string.IsNullOrEmpty(this.Destination.ProviderName))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.ProviderName"));
            }

            if (string.IsNullOrEmpty(this.Destination.TableName))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.TableName"));
            }

            if (this.Source.FieldDelimiter.Length != 1)
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyTooLong, "Source.FieldDelimiter", 1));
            }

            if (this.Source.Quote != null && this.Source.Quote.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyTooLong, "Source.Quote", 1));
            }

            if (this.Source.Escape != null && this.Source.Escape.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyTooLong, "Source.Escape", 1));
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
                                LineDelimiter1 =
                                    this.Source.LineDelimiter != null
                                        ? this.Source.LineDelimiter[0]
                                        : DefaultLineDelimiter[0], 
                                LineDelimiter2 =
                                    this.Source.LineDelimiter != null
                                        ? this.Source.LineDelimiter[1]
                                        : DefaultLineDelimiter[1], 
                            };

            var files = string.IsNullOrEmpty(this.Source.FileMask)
                ? Directory.GetFiles(this.Source.FilePath)
                : Directory.GetFiles(this.Source.FilePath, this.Source.FileMask);

            var invalidMappingCount = 0;

            foreach (var fileName in files)
            {
                using (var fileReader = new StreamReader(fileName, Encoding.GetEncoding(this.Source.CodePage)))
                {
                    using (var csvReader = new CsvReader(fileReader, csvSyntax))
                    {
                        var existColumns = csvReader.GetFieldHeaders();

                        if (this.Mappings.Count > 0)
                        {
                            var excepts =
                                this.Mappings.Where(x => x.SourceFieldName != null)
                                    .Select(x => x.SourceFieldName)
                                    .Except(existColumns, StringComparer.Create(CultureInfo.InvariantCulture, true))
                                    .ToArray();

                            if (excepts.Length > 0)
                            {
                                invalidMappingCount++;
                                var errorDateTime = DateTime.Now;

                                var mess = string.Format(
                                    "File {0} not has column(s): {1}. Skipped.",
                                    fileName,
                                    string.Join(", ", excepts));

                                logger.LogEtlMessage(
                                    new EtlMessage
                                    {
                                        EtlPackageId = context.EtlPackageId,
                                        EtlSessionId = context.EtlSessionId,
                                        EtlStepName = this.Name,
                                        LogDateTime = errorDateTime,
                                        LogUtcDateTime = errorDateTime.ToUniversalTime(),
                                        MessageType = EtlMessageType.Error,
                                        Text = mess
                                    });
                                continue;
                            }
                        }
                        else
                        {
                            this._mappings = this.GetDefaultMappings(existColumns);
                        }

                        var wrappedReader = new EtlMappedDataReader(csvReader, this.Mappings);

                        var dbWriter = new DBTableWriter(
                            this.Destination.ConnectionString, 
                            this.Destination.ProviderName, 
                            this.Destination.TableName);

                        dbWriter.ErrorOccured += delegate(object sender, DBTableWriterErrorEventArgs e)
                        {
                            var errorDateTime = DateTime.Now;
                            result.Status = EtlStatus.FinishedWithLosses;
                            result.Message = e.Message;

                            // todo: fix error counter in CSV import. Now it counts errors, not error records
                            errorRowCount++;

                            logger.LogEtlMessage(
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
                                });

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

                        var counterDateTime = DateTime.Now;

                        logger.LogEtlCounter(new EtlCounter
                        {
                            EtlPackageId = context.EtlPackageId,
                            EtlSessionId = context.EtlSessionId,
                            EntityName = Path.GetFileName(fileName),
                            CounterName = "RowCount",
                            CounterValue = sourceRowCount,
                            DateTime = counterDateTime,
                            UtcDateTime = counterDateTime.ToUniversalTime()
                        });
                    }
                }
            }

            var nowTime = DateTime.Now;

            if (files.Length > 0 && invalidMappingCount == files.Length)
            {
                // NOTE: Если все файлы имеют ошибки маппинга, то прерываем обработку.
                logger.LogEtlMessage(
                    new EtlMessage
                        {
                            EtlPackageId = context.EtlPackageId,
                            EtlSessionId = context.EtlSessionId,
                            LogDateTime = nowTime,
                            LogUtcDateTime = nowTime.ToUniversalTime(),
                            MessageType = EtlMessageType.Error,
                            Text = "All files have mappings errors. Break processing."
                        });
                result.Status = EtlStatus.Failed;
                return result;
            }

            if (invalidMappingCount > 0 && result.Status == EtlStatus.Succeeded)
            {
                result.Status = EtlStatus.FinishedWithLosses;
            }

            logger.LogEtlMessage(
                new EtlMessage
                {
                    EtlPackageId = context.EtlPackageId, 
                    EtlSessionId = context.EtlSessionId, 
                    LogDateTime = nowTime, 
                    LogUtcDateTime = nowTime.ToUniversalTime(), 
                    MessageType = EtlMessageType.Information, 
                    Text =
                        string.Format(
                            "Source:{0} Inserted:{1} Errors:{2}", sourceRowCount, insertedRowCount, errorRowCount)
                });

            return result;
        }

        private List<EtlFieldMapping> GetDefaultMappings(string[] existColumns)
        {
            return
                existColumns.Select(
                    colName => new EtlFieldMapping { SourceFieldName = colName, DestinationFieldName = colName })
                    .ToList();
        }

        #endregion

        //internal class StringComparer : IEqualityComparer<string>
        //{
            
        //}
    }
}