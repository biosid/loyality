namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.DataSources.Csv;
    using RapidSoft.Etl.Runtime.DataSources.DB;
    using RapidSoft.Etl.Runtime.Properties;

    [Serializable]
    public sealed class EtlExportCsvFileStep : EtlStep
    {
        #region Constants

        public static readonly char DefaultQuote = '"';

        public static readonly char DefaultEscape = '"';

        public static readonly string DefaultLineDelimiter = "\r\n";

        #endregion

        #region Properties

        private List<EtlFieldMapping> _mappings = new List<EtlFieldMapping>();

        [Category("1. General")]
        public bool EndSessionOnEmptySource
        {
            get;
            set;
        }

        [Category("2. Source")]
        public EtlQuerySourceInfo Source
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public EtlCsvFileInfo Destination
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

            if (string.IsNullOrEmpty(this.Source.ConnectionString))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.ConnectionString"));
            }

            if (string.IsNullOrEmpty(this.Source.ProviderName))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Source.ProviderName"));
            }

            if (string.IsNullOrEmpty(this.Source.Text))
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeNull, "Source.Text"));
            }

            if (string.IsNullOrEmpty(this.Destination.FieldDelimiter))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.FieldDelimiter"));
            }

            if (string.IsNullOrEmpty(this.Destination.FilePath))
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyCannotBeNull, "Destination.FilePath"));
            }

            if (this.Destination.FieldDelimiter.Length != 1)
            {
                throw new InvalidOperationException(
                    string.Format(Resources.PropertyTooLong, "Destination.FieldDelimiter", 1));
            }

            if (this.Destination.Quote != null && this.Destination.Quote.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyTooLong, "Destination.Quote", 1));
            }

            if (this.Destination.Escape != null && this.Destination.Escape.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Resources.PropertyTooLong, "Destination.Escape", 1));
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);

            var sourceRowCount = 0L;
            var writtenRowCount = 0L;

            var encoding = Encoding.GetEncoding(this.Destination.CodePage);

            var csvSyntax = new CsvSyntaxInfo
                            {
                                HasHeaders = this.Destination.HasHeaders, 
                                FieldDelimiter = this.Destination.FieldDelimiter[0], 
                                Quote = this.Destination.Quote != null ? this.Destination.Quote[0] : DefaultQuote, 
                                Escape = this.Destination.Escape != null ? this.Destination.Escape[0] : DefaultEscape, 
                                LineDelimiter1 =
                                    this.Destination.LineDelimiter != null
                                        ? this.Destination.LineDelimiter[0]
                                        : DefaultLineDelimiter[0], 
                                LineDelimiter2 =
                                    this.Destination.LineDelimiter != null
                                        ? this.Destination.LineDelimiter[1]
                                        : DefaultLineDelimiter[1], 
                            };

            if (!Directory.Exists(Path.GetDirectoryName(this.Destination.FilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this.Destination.FilePath));
            }

            using (var dbAccessor = new DBAccessor(this.Source.ConnectionString, this.Source.ProviderName))
            {
                using (
                    var dbReader = dbAccessor.ExecuteQuery(
                        this.Source.Text, 
                        EtlQueryParameter.ToDictionary(this.Source.Parameters), 
                        this.TimeoutMilliseconds))
                {
                    if (this.Mappings.Count == 0)
                    {
                        this._mappings = this.GetDefaultMappings(dbReader);
                    }

                    using (var mapReader = new EtlMappedDataReader(dbReader, this.Mappings))
                    {
                       var counterDataSource =
                           new FileCounterDataSource(
                               this.Destination.FileCounterDbStorage.ConnectionString,
                               context.EtlPackageId,
                               context.EtlSessionId,
                               Path.GetFileName(this.Destination.FilePath),
                               this.Destination.FileCounterDbStorage.SchemaName);

                        var nextFileNumber = counterDataSource.GetNextFileNumber();

                        var fileName =
                            this.Destination.FilePath.Replace(
                                this.Destination.FileCounterDbStorage.BatchCounterTag,
                                nextFileNumber.ToString());

                        using (var fileWriter = new StreamWriter(fileName, false, encoding))
                        {
                            var csvWriter = new CsvWriter(fileWriter, csvSyntax, null);

                            var t = false;
                            writtenRowCount = csvWriter.Write(mapReader, ref t);                                                        
                            sourceRowCount = writtenRowCount;
                        }

                        if (this.EndSessionOnEmptySource && writtenRowCount > 0)
                        {
                            counterDataSource.SaveFileNumber(nextFileNumber);
                        }

                        var counterDateTime = DateTime.Now;

                        logger.LogEtlCounter(new EtlCounter
                        {
                            EtlPackageId = context.EtlPackageId,
                            EtlSessionId = context.EtlSessionId,
                            EntityName = Path.GetFileName(fileName),
                            CounterName = "RowCount",
                            CounterValue = writtenRowCount,
                            DateTime = counterDateTime,
                            UtcDateTime = counterDateTime.ToUniversalTime()
                        });
                    }
                }
            }

            // logger.LogEtlMessage(new EtlMessage
            // {
            // EtlPackageId = context.EtlPackageId,
            // EtlSessionId = context.EtlSessionId,
            // EtlStepId = this.Id,
            // LogDateTime = endDateTime,
            // LogUtcDateTime = endDateTime.ToUniversalTime(),
            // MessageType = EtlMessageType.Statistics,
            // Text = "Found",
            // Flags = sourceRowCount,
            // });

            // logger.LogEtlMessage(new EtlMessage
            // {
            // EtlPackageId = context.EtlPackageId,
            // EtlSessionId = context.EtlSessionId,
            // EtlStepId = this.Id,
            // LogDateTime = endDateTime,
            // LogUtcDateTime = endDateTime.ToUniversalTime(),
            // MessageType = EtlMessageType.Statistics,
            // Text = "Exported",
            // Flags = writtenRowCount,
            // });

            if (this.EndSessionOnEmptySource && writtenRowCount == 0)
            {
                return new EtlStepResult(EtlStatus.FinishedWithSessionEnd, "End session because of empty source");
            }

            return result;
        }

        private List<EtlFieldMapping> GetDefaultMappings(IDataReader dbReader)
        {
            return Enumerable.Range(0, dbReader.FieldCount)
                    .Select(dbReader.GetName)
                    .Select(colName => new EtlFieldMapping { SourceFieldName = colName, DestinationFieldName = colName })
                    .ToList();
        }

        private void LogMessage(EtlContext context, IEtlLogger logger, string message)
        {
            logger.LogEtlMessage(
                new EtlMessage
                {
                    EtlPackageId = context.EtlPackageId, 
                    EtlSessionId = context.EtlSessionId, 
                    LogDateTime = DateTime.Now, 
                    LogUtcDateTime = DateTime.UtcNow, 
                    MessageType = EtlMessageType.Information, 
                    Text = message, 
                });
        }

        private static DbConnection CreateConnection(string connectionString, string providerName)
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;

            return connection;
        }

        private static IDataReader CreateTableReader(DbConnection connection, string queryText, int timeout)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = queryText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = timeout;

            var reader = cmd.ExecuteReader();
            return reader;
        }

        #endregion
    }
}