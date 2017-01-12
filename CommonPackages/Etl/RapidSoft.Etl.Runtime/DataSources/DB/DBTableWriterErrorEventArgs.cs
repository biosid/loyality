using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    [Serializable]
    internal sealed class DBTableWriterErrorEventArgs : EventArgs
    {
        #region Constructors

        public DBTableWriterErrorEventArgs(DBTableWriterErrorFlags flags, long recordIndex, Exception exception)
            : this(flags, recordIndex, exception, null)
        {
        }

        public DBTableWriterErrorEventArgs(DBTableWriterErrorFlags flags, long recordIndex, Exception exception, string message)
        {
            _flags = flags;

            if (message == null && exception != null)
            {
                message = exception.Message;
            }

            _recordIndex = recordIndex;
            _message = message;
            _exception = exception;
        }
    
        #endregion

        #region Fields

        private readonly DBTableWriterErrorFlags _flags;
        private readonly string _message;
        private readonly long _recordIndex;
        private readonly Exception _exception;

        private bool _trySkipError;

        #endregion

        #region Properties

        public bool TrySkipError
        {
            [DebuggerStepThrough]
            get
            {
                return _trySkipError;
            }
            [DebuggerStepThrough]
            set
            {
                _trySkipError = value;
            }
        }

        public long RecordIndex
        {
            [DebuggerStepThrough]
            get
            {
                return _recordIndex;
            }
        }

        public string Message
        {
            [DebuggerStepThrough]
            get
            {
                return _message;
            }
        }

        public Exception Exception
        {
            [DebuggerStepThrough]
            get
            {
                return _exception;
            }
        }

        public bool IsReadError
        {
            get
            {
                return (_flags & DBTableWriterErrorFlags.ReadError) == DBTableWriterErrorFlags.ReadError;
            }
        }

        public bool IsWriteError
        {
            get
            {
                return (_flags & DBTableWriterErrorFlags.WriteError) == DBTableWriterErrorFlags.WriteError;
            }
        }
        #endregion
    }
}
