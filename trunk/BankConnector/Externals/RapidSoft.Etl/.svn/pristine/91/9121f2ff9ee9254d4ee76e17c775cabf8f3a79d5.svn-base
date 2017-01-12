using System;
using System.Diagnostics;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [DebuggerDisplay("{Name} = {Value}")]
    public sealed class EtlRequestHeader
    {
        #region Constructors

        public EtlRequestHeader()
        {
        }

        public EtlRequestHeader(string name, string value)
        {
            this.Name = name;
            this.Value = value;
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
    }
}
