using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public class EtlContext
    {
        #region Constructors

        public EtlContext
        (
            EtlSession session
        )
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _parentSessionId = session.ParentEtlSessionId;
            _packageId = session.EtlPackageId;
            _sessionId = session.EtlSessionId;
            _userName = session.UserName;
            _startDateTime = session.StartDateTime ?? DateTime.Now;
            _startUtcDateTime = session.StartUtcDateTime ?? _startDateTime.ToUniversalTime();
        }

        #endregion

        #region Fields

        private readonly string _parentSessionId;
        private readonly string _packageId;
        private readonly string _sessionId;
        private readonly string _userName;
        private readonly DateTime _startDateTime;
        private readonly DateTime _startUtcDateTime;

        #endregion

        #region Properties

        public string ParentEtlSessionId
        {
            [DebuggerStepThrough]
            get
            {
                return _parentSessionId;
            }
        }

        public string EtlPackageId
        {
            [DebuggerStepThrough]
            get
            {
                return _packageId;
            }
        }

        public string EtlSessionId
        {
            [DebuggerStepThrough]
            get
            {
                return _sessionId;
            }
        }

        public string UserName
        {
            [DebuggerStepThrough]
            get
            {
                return _userName;
            }
        }

        public DateTime StartDateTime
        {
            [DebuggerStepThrough]
            get
            {
                return _startDateTime;
            }
        }

        public DateTime StartUtcDateTime
        {
            [DebuggerStepThrough]
            get
            {
                return _startUtcDateTime;
            }
        }

        #endregion
    }
}
