using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlDelayStep : EtlStep
    {
        #region Constructors

        public EtlDelayStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Parameters")]
        public int DelayMilliseconds
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

            if (this.DelayMilliseconds > 0)
            {
                Thread.Sleep(this.DelayMilliseconds);
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        #endregion
    }
}
