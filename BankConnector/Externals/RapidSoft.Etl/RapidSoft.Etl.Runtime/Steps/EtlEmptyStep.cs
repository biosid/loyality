using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlEmptyStep : EtlStep
    {
        #region Constructors

        public EtlEmptyStep()
        {
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
            return result;
        }

        #endregion
    }
}
