using System;
using System.Runtime.Serialization;

namespace RapidSoft.Etl.Editor
{
    [Serializable]
    public class ExceptionInfo
    {
        #region Constuctors

        public ExceptionInfo()
        {
        }

        public ExceptionInfo(Exception exc)
            : this(exc, null)
        {
        }

        public ExceptionInfo(Exception exc, string details)
            : this(exc, details, null)
        {
        }

        public ExceptionInfo(Exception exc, string details, string[] contextValues)
            : this(0, exc, details, contextValues)
        {
        }

        protected ExceptionInfo(int innerExceptionLevel, Exception exc, string details, string[] contextValues)
        {
            this.Details = details;
            this.ContextValues = contextValues;
            this.LocalDateTime = DateTime.Now.ToString("O");

            if (exc != null)
            {
                this.Message = exc.Message;
                this.ExceptionTypeName = exc.GetType().FullName;
                this.Source = exc.Source;
                this.StackTrace = exc.StackTrace;

                if (exc.InnerException != null)
                {
                    innerExceptionLevel++;

                    if (innerExceptionLevel < MAX_INNER_EXCEPTION_LEVEL)
                    {
                        this.InnerExceptionInfo = new ExceptionInfo(innerExceptionLevel, exc.InnerException, null, null);
                    }
                }
            }
        }

        public ExceptionInfo(string message, string details)
            : this(message, details, null)
        {
        }

        public ExceptionInfo(string message, string details, string[] contextValues)
        {
            this.Message = message;
            this.Details = details;
            this.ContextValues = contextValues;
            this.LocalDateTime = DateTime.Now.ToString("O");
        }

        #endregion

        #region Constants

        private const int MAX_INNER_EXCEPTION_LEVEL = 8;

        #endregion

        #region Properties

        public string Message
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public string[] ContextValues
        {
            get;
            set;
        }

        public string ExceptionTypeName
        {
            get;
            set;
        }

        public string LocalDateTime
        {
            get;
            set;
        }

        public string Source
        {
            get;
            set;
        }

        public string StackTrace
        {
            get;
            set;
        }

        public ExceptionInfo InnerExceptionInfo
        {
            get;
            set;
        }

        #endregion

        #region Methods

        //public string GetInnerExceptionText()
        //{
        //    this.InnerExceptionText = "";

        //    var inner = exc.InnerException;
        //    while (inner != null)
        //    {
        //        this.InnerExceptionText += inner.ToString() + ";";
        //        inner = inner.InnerException;
        //    }
        //}

        public override string ToString()
        {
            return this.Message;
        }

        #endregion
    }
}