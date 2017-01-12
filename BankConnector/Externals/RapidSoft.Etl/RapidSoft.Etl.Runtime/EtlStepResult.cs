using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public sealed class EtlStepResult
    {
        #region Constructors

        public EtlStepResult()
        {
        }

        public EtlStepResult(EtlStatus stepStatus, string message)
            : this(stepStatus, message, null)
        {
        }

        public EtlStepResult(EtlStatus stepStatus, string message, EtlVariableAssignment[] variableAssignments)
        {
            this.Status = stepStatus;
            this.Message = message;

            if (variableAssignments != null)
            {
                this.VariableAssignments.AddRange(variableAssignments);
            }
        }

        #endregion

        #region Properties

        public EtlStatus Status
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public List<EtlVariableAssignment> VariableAssignments
        {
            [DebuggerStepThrough]
            get
            {
                return _variableAssignments;
            }
        }
        private readonly List<EtlVariableAssignment> _variableAssignments = new List<EtlVariableAssignment>();

        #endregion
    }
}
