namespace RapidSoft.GeoPoints
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

        protected OperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string ResultDescription { get; set; }
    }
}