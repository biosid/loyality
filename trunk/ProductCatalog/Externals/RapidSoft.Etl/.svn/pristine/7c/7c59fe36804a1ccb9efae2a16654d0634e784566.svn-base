using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Data.Common;
using System.Data;

using RapidSoft.Etl.Runtime.DataSources.DB;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlExecuteProcedureStep : EtlStep
    {
        #region Constructors

        public EtlExecuteProcedureStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlProcedureSourceInfo Source
        {
            get;
            set;
        }

        [Browsable(false)]
        public EtlExecuteQueryOutputVariableSet Statistics
        {
            get;
            set;
        }

        [Category("3. Output")]
        public EtlExecuteProcedureOutputVariableSet OutputVariables
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

            if (string.IsNullOrEmpty(this.Source.ConnectionString))
            {
                throw new InvalidOperationException("ConnectionString cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.ProviderName))
            {
                throw new InvalidOperationException("ProviderName cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.ProcedureName))
            {
                throw new InvalidOperationException("ProcedureName cannot be empty");
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);
            var hasOutputVariables = this.OutputVariables != null && this.OutputVariables.FirstRow.Count > 0;

            using (var dbAccessor = new DBAccessor(this.Source.ConnectionString, this.Source.ProviderName))
            {
                if (hasOutputVariables)
                {
                    using (var dbReader = dbAccessor.ExecuteProcedureReader(this.Source.ProcedureName, EtlProcedureParameter.ToDictionary(this.Source.Parameters), this.TimeoutMilliseconds))
                    {
                        if (dbReader.Read())
                        {
                            foreach (var firstRowResult in this.OutputVariables.FirstRow)
                            {
                                var firstRowResultValue = EtlValueTranslation.Evaluate(firstRowResult.SourceFieldName, firstRowResult.SourceFieldTranslation, dbReader, firstRowResult.DefaultValue);
                                result.VariableAssignments.Add(new EtlVariableAssignment(firstRowResult.VariableName, EtlValueConverter.ToString(firstRowResultValue)));
                            }
                        }
                    }
                }
                else
                {
                    dbAccessor.ExecuteProcedure(this.Source.ProcedureName, EtlProcedureParameter.ToDictionary(this.Source.Parameters), this.TimeoutMilliseconds);
                }
            }

            return result;
        }

        #endregion
    }
}
