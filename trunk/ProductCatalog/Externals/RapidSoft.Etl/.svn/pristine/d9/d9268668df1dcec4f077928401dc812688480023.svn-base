using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
    [DebuggerDisplay("{Name} = {Value}")]
    public class EtlVariable : ICloneable
    {
        #region Constructors

        public EtlVariable()
        { 
        }

        protected EtlVariable(EtlVariable obj)
        {
            this.EtlPackageId = obj.EtlPackageId;
            this.EtlSessionId = obj.EtlSessionId;
            this.Name = obj.Name;
            this.Modifier = obj.Modifier;
            this.Value = obj.Value;
            this.IsSecure = obj.IsSecure;
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

        public string Name
        {
            get;
            set;
        }

        public EtlVariableModifier Modifier
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public bool IsSecure
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
            return new EtlVariable(this);
        }

        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}
