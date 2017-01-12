using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
	public sealed class EtlSessionQuery
    {
        #region Constructors

        public EtlSessionQuery()
        {
        }

        #endregion

        #region Properties

        public DateTime FromDateTime
        {
            get;
            set;
        }

        public DateTime ToDateTime
        {
            get;
            set;
        }

        [XmlArrayItem("PackageId")]
        public List<string> EtlPackageIds
        {
            [DebuggerStepThrough]
            get
            {
                return _packageIds;
            }
        }
        private readonly List<string> _packageIds = new List<string>();

        public List<EtlStatus> EtlStatuses
        {
            [DebuggerStepThrough]
            get
            {
                return _etlStatuses;
            }
        }
        private readonly List<EtlStatus> _etlStatuses = new List<EtlStatus>();

        //[XmlArrayItem("UserName")]
        //public List<string> UserNames
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return _userNames;
        //    }
        //}
        //private readonly List<string> _userNames = new List<string>();

        [XmlArrayItem("Variable")]
        public List<EtlVariableFilter> Variables
        {
            [DebuggerStepThrough]
            get
            {
                return _variables;
            }
        }
        private readonly List<EtlVariableFilter> _variables = new List<EtlVariableFilter>();

        public int? MaxSessionCount
        {
            get;
            set;
        }

        #endregion
    }
}