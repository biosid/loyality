using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RapidSoft.Etl.Editor
{
    public sealed class PackageCopiedEventArgs : EventArgs
    {
        #region Constructors

        public PackageCopiedEventArgs(string etlPackageId)
        {
            _etlPackageId = etlPackageId;
        }

        #endregion

        #region Fields

        private readonly string _etlPackageId;

        #endregion

        #region Properties

        public string EtlPackageId
        {
            [DebuggerStepThrough]
            get
            {
                return _etlPackageId;
            }
        }

        #endregion
    }
}
