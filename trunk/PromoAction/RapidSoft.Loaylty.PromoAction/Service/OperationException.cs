namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

    public class OperationException : Exception
    {
        public OperationException()
        {
        }

        public OperationException(string message)
            : base(message)
        {
            this.ResultCode = ResultCodes.UNKNOWN_ERROR;
        }

        public OperationException(int resultCode, string message)
            : base(message)
        {
            this.ResultCode = resultCode;
        }

        public OperationException(string message, Exception inner)
            : base(message, inner)
        {
            this.ResultCode = ResultCodes.UNKNOWN_ERROR;
        }

        public OperationException(int resultCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ResultCode = resultCode;
        }

        [DataMember]
        public int ResultCode { get; set; }
    }
}