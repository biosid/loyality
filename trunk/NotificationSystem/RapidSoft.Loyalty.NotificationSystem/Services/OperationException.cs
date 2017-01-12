namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    using System;
    using System.Runtime.Serialization;

    public class OperationException : Exception
    {
        public OperationException()
        {
        }

        public OperationException(string message)
            : base(message)
        {
        }

        public OperationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public OperationException(int resultCode, string resultDescription, Exception inner = null)
            : base(resultDescription, inner)
        {
            ResultDescription = resultDescription;
            ResultCode = resultCode;
        }

        protected OperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [DataMember]
        public int ResultCode
        {
            get;
            set;
        }

        [DataMember]
        public string ResultDescription
        {
            get;
            set;
        }
    }
}