using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;

using RapidSoft.Etl.Runtime.Properties;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlFailStep : EtlStep
    {
        #region Properties

        [Category("1. General")]
        public string Message
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            var result = new EtlStepResult(EtlStatus.Succeeded, null);

            return new EtlStepResult(EtlStatus.Failed, this.Message);
        }

        #endregion
    }
}
