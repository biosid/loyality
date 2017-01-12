using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
    [DebuggerDisplay("{Name} = {Value}")]
    public class EtlVariableFilter
    {
        #region Constructors

        public EtlVariableFilter()
        { 
        }

        public EtlVariableFilter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        protected EtlVariableFilter(EtlVariableFilter obj)
        {
            this.Name = obj.Name;
            this.Value = obj.Value;
        }

        #endregion

        #region Properties

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public object Clone()
        {
            return new EtlVariableFilter(this);
        }

        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}
