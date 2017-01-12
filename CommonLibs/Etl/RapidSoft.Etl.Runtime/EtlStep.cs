using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public abstract class EtlStep
    {
        #region Constructors

        public EtlStep()
        {
        }

        #endregion

        #region Properties

        [Category("1. General")]
        [XmlElement(IsNullable = false)]
        [DisplayName("(Name)")]
        public string Name
        {
            get;
            set;
        }

        [Category("1. General")]
        public string Description
        {
            get;
            set;
        }

        [Category("1. General")]
        public int? TimeoutMilliseconds
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public abstract EtlStepResult Invoke(EtlContext context, IEtlLogger logger);

        #endregion
    }
}
