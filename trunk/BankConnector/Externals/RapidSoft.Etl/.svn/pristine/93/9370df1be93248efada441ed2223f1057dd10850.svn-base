using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
    [DebuggerDisplay("{CounterName} = {CounterValue}")]
    public class EtlCounter : ICloneable
    {
        #region Constructors

        public EtlCounter()
        { 
        }

        protected EtlCounter(EtlCounter obj)
        {
            this.EtlPackageId = obj.EtlPackageId;
            this.EtlSessionId = obj.EtlSessionId;
            this.EntityName = obj.EntityName;
            this.CounterName = obj.CounterName;
            this.CounterValue = obj.CounterValue;
            this.DateTime = obj.DateTime;
            this.UtcDateTime = obj.UtcDateTime;
        }

        #endregion

        #region Properties

        public string EtlPackageId
        {
            get;
            set;
        }

        public string EtlSessionId
        {
            get;
            set;
        }

        public string EntityName
        {
            get;
            set;
        }

        public string CounterName
        {
            get;
            set;
        }

        public long CounterValue
        {
            get;
            set;
        }

        public DateTime DateTime
        {
            get;
            set;
        }

        public DateTime? UtcDateTime
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public object Clone()
        {
            return new EtlCounter(this);
        }

        public override string ToString()
        {
            return this.CounterValue.ToString();
        }

        #endregion
    }
}
