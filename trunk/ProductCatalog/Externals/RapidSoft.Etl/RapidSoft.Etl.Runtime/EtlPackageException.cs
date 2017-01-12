using System;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public sealed class EtlPackageException : Exception
    {
        #region Constructors

        public EtlPackageException()
        {
        }

        public EtlPackageException(string message)
            : base(message)
        {
        }

        public EtlPackageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
