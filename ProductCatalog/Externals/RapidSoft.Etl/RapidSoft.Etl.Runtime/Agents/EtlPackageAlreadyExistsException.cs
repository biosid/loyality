using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Agents
{
	[Serializable]
	public sealed class EtlPackageAlreadyExistsException : ApplicationException
    {
        #region Constructors

        public EtlPackageAlreadyExistsException()
            : this(null, null, null)
        {
        }

        public EtlPackageAlreadyExistsException(string message)
            : this(null, message, null)
        {
        }

        public EtlPackageAlreadyExistsException(string message, Exception innerException)
            : this(null, message, innerException)
        {
        }

        public EtlPackageAlreadyExistsException(string etlPackageId, string message)
            : this(etlPackageId, message, null)
        {
        }

        public EtlPackageAlreadyExistsException(string etlPackageId, string message, Exception innerException)
            : base(message, innerException)
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