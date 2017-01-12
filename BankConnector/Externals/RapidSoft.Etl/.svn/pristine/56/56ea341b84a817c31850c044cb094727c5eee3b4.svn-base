using System;
using System.Diagnostics;
using System.ComponentModel;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    [DebuggerDisplay("{Name} = {Value}")]
    public class EtlVariableAssignment : ICloneable
    {
        #region Constructors

        public EtlVariableAssignment()
        { 
        }

        public EtlVariableAssignment(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        protected EtlVariableAssignment(EtlVariableAssignment obj)
        {
            this.Name = obj.Name;
            this.Value = obj.Value;
        }

        #endregion

        #region Properties

        [DisplayName("(Name)")]
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
            return new EtlVariableAssignment(this);
        }

        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}
