using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.DataSources.Xml;
using RapidSoft.Etl.Runtime.DataSources.DB;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add statistics
    [Serializable]
    public sealed class EtlImportFlatXmlFileStep : EtlStep
    {
        #region Constructors

        public EtlImportFlatXmlFileStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlXmlFileInfo Source
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

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException("Source.FilePath cannot be empty");
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

            var fieldNames = GetFieldNames(this.Mappings);

            var sourceRowCount = 0L;
            var insertedRowCount = 0L;
            var errorRowCount = 0L;

            using (var xmlReader = new XmlTextReader(this.Source.FilePath))
            {
                using (var xmlDataReader = new FlatXmlDataReader(
                    xmlReader,
                    this.Source.DataElementPath,
                    fieldNames,
                    new FlatXmlDataReaderOptions
                    {
                        TreatEmptyStringAsNull = this.Source.TreatEmptyStringAsNull
                    }
                ))
                {
                    var wrappedReader = new EtlMappedDataReader(xmlDataReader, this.Mappings);
                    var writer = new DBTableWriter(this.Destination.ConnectionString, this.Destination.ProviderName, this.Destination.TableName);

                    writer.ErrorOccured += delegate(object sender, DBTableWriterErrorEventArgs e)
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
                            if (e.IsWriteError)
                            {
                                result.Status = EtlStatus.FinishedWithLosses;
                                e.TrySkipError = true;
                            }
                            else
                            {
                                result.Status = EtlStatus.Failed;
                                e.TrySkipError = false;
                            }
                        }
                        else
                        {
                            result.Status = EtlStatus.Failed;
                        }
                    };

                    insertedRowCount = writer.Write(wrappedReader, this.TimeoutMilliseconds, this.BatchSize);
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

        private static string[] GetFieldNames(IList<EtlFieldMapping> mappings)
        {
            var fieldNames = new string[mappings.Count];
            for (var i = 0; i < fieldNames.Length; i++)
            {
                fieldNames[i] = mappings[i].SourceFieldName;
            }
            return fieldNames;
        }

        #endregion
    }
}
