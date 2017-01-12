namespace RapidSoft.Loaylty.PartnersConnector.Services
{
    using System;
    using System.Runtime.Serialization;

    public class OperationException : Exception
    {
        public OperationException(int resultCode, string message)
            : base(message)
        {
            this.ResultCode = resultCode;
            this.ResultDescription = message;
        }

        public OperationException(int resultCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ResultCode = resultCode;
            this.ResultDescription = message;
        }

        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string ResultDescription { get; set; }
    }
}