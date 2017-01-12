namespace RapidSoft.Loaylty.ProductCatalog.Services
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

        public OperationException(string message, int resultCode, Exception inner = null)
            : base(message, inner)
        {
            ResultCode = resultCode;
        }

        public OperationException(int resultCode, string resultDescription, Exception inner = null)
            : base(resultDescription, inner)
        {
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
    }
}