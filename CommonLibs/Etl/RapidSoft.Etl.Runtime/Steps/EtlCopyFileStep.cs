using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public sealed class EtlCopyFileStep : EtlStep
    {
        #region Constructors

        public EtlCopyFileStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlFileInfo Source
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public EtlFileInfo Destination
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

            if (this.Source == null)
            {
                throw new InvalidOperationException("Source cannot be null");
            }

            if (this.Destination == null)
            {
                throw new InvalidOperationException("Destination cannot be null");
            }

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException("Source.FilePath cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Destination.FilePath))
            {
                throw new InvalidOperationException("Destination.FilePath cannot be empty");
            }

            var destinationFolderPath = Path.GetDirectoryName(this.Destination.FilePath);

            if (!string.IsNullOrEmpty(destinationFolderPath))
            {
                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }
            }

            File.Copy(this.Source.FilePath, this.Destination.FilePath);

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        #endregion
    }
}
