using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public sealed partial class EtlPackage
    {
        #region Properties

        [Category("1. General")]
        [DisplayName("(Id)")]
        public string Id
        {
            get;
            set;
        }

        [Category("1. General")]
        [DisplayName("(Name)")]
        public string Name
        {
            get;
            set;
        }

        [Category("1. General")]
        public int RunIntervalSeconds
        {
            get;
            set;
        }

        [Category("1. General")]
        public bool Enabled
        {
            get;
            set;
        }

        [Category("2. Variables")]
        [XmlArrayItem("Variable")]
        public List<EtlVariableInfo> Variables
        {
            [DebuggerStepThrough]
            get
            {
                return _variables;
            }
        }
        private readonly List<EtlVariableInfo> _variables = new List<EtlVariableInfo>();

        //[Browsable(false)]
        //[XmlArray("Parameters")]
        //[XmlArrayItem("Parameter")]
        //public List<EtlVariableInfo> Obsolete_Parameters
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return _variables;
        //    }
        //}

        [Category("3. Steps")]
        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        public List<EtlStep> Steps
        {
            [DebuggerStepThrough]
            get
            {
                return _steps;
            }
        }
        private readonly List<EtlStep> _steps = new List<EtlStep>();

        #endregion

        #region Methods

        public EtlSession Invoke(IEtlLogger logger)
        {
            return Invoke(logger, null, null);
        }

        public EtlSession Invoke(IEtlLogger[] loggers)
        {
            return Invoke(loggers, null, null);
        }

        public EtlSession Invoke(IEtlLogger logger, EtlVariableAssignment[] variables)
        {
            return Invoke(logger, variables, null);
        }

        public EtlSession Invoke(IEtlLogger[] loggers, EtlVariableAssignment[] variables)
        {
            return Invoke(loggers, variables, null);
        }

        public EtlSession Invoke(IEtlLogger logger, EtlVariableAssignment[] variables, string parentSessionId, string user = null, EtlSession waitingEtlSession = null)
        {
            var invoker = new _Invoker(logger);
            var session = invoker.InvokePackage(this, variables, parentSessionId, user, waitingEtlSession);
            return session;
        }

        public EtlSession Invoke(IEtlLogger[] loggers, EtlVariableAssignment[] variables, string parentSessionId, string user = null, EtlSession waitingEtlSession = null)
        {
            var logger = JoinLoggers(loggers);
            return Invoke(logger, variables, parentSessionId, user, waitingEtlSession);
        }

        private static IEtlLogger JoinLoggers(IEtlLogger[] loggers)
        {
            if (loggers == null)
            {
                return new NullEtlLogger();
            }
            else if (loggers.Length == 1)
            {
                return loggers[0];
            }
            else
            {
                return new MultiEtlLogger(loggers);
            }
        }

        #endregion
    }
}
