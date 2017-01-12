using System;
using System.ComponentModel;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public abstract class EtlDataSourceInfo
    {
        #region Properties

        [DisplayName("(Name)")]
        public string Name
        {
            get;
            set;
        }

        #endregion
    }
}
