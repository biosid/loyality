using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.DataSources.DB;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add statistics
    [Serializable]
    public sealed class EtlImportQueryStep : EtlStep
    {
        #region Constructors

        public EtlImportQueryStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlQuerySourceInfo Source
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
                throw new InvalidOperationException("Source cannot be null");
            }

            if (this.Destination == null)
            {
                throw new InvalidOperationException("Destination cannot be null");
            }

            if (string.IsNullOrEmpty(this.Source.ConnectionString))
            {
                throw new InvalidOperationException("Source.ConnectionString cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.ProviderName))
            {
                throw new InvalidOperationException("Source.ProviderName cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.Text))
            {
                throw new InvalidOperationException("Source.Text cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Destination.ConnectionString))
            {
                throw new InvalidOperationException("Destination.ConnectionString cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Destination.ProviderName))
            {
                throw new InvalidOperationException("Destination.ProviderName cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Destination.TableName))
            {
                throw new InvalidOperationException("Destination.TableName cannot be empty");
            }

            if (this.Mappings.Count == 0)
            {
                throw new InvalidOperationException("Mappings not specified");
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);

            var sourceRowCount = 0L;
            var insertedRowCount = 0L;
            var errorRowCount = 0L;

            using (var dbAccessor = new DBAccessor(this.Source.ConnectionString, this.Source.ProviderName))
            {
                using (var dbReader = dbAccessor.ExecuteQuery(this.Source.Text, EtlQueryParameter.ToDictionary(this.Source.Parameters), this.TimeoutMilliseconds))
                {
                    using (var mapReader = new EtlMappedDataReader(dbReader, this.Mappings))
                    {
                        var dbWriter = new DBTableWriter(this.Destination.ConnectionString, this.Destination.ProviderName, this.Destination.TableName);
                        dbWriter.ErrorOccured += delegate(object sender, DBTableWriterErrorEventArgs e)
                        {
                            var errorDateTime = DateTime.Now;
                            result.Status = EtlStatus.FinishedWithLosses;
                            result.Message = e.Message;

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

                        insertedRowCount = dbWriter.Write(mapReader, this.TimeoutMilliseconds, this.BatchSize);
                    }
                }
            }

            sourceRowCount = insertedRowCount + errorRowCount;

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
