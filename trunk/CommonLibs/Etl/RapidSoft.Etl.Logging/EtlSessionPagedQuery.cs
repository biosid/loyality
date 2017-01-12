namespace RapidSoft.Etl.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public sealed class EtlSessionPagedQuery
    {
        #region Constructors

        public EtlSessionPagedQuery()
        {
            EtlPackageIds = new List<string>();
            Variables = new List<EtlVariableFilter>();
        }

        #endregion

        public int CountToSkip { get; set; }

        public int CountToTake { get; set; }

        public bool CalcCount { get; set; }

        [XmlArrayItem("PackageId")]
        public List<string> EtlPackageIds
        {
            get;
            private set;
        }

        [XmlArrayItem("Variable")]
        public List<EtlVariableFilter> Variables
        {
            get;
            private set;
        }
    }
}