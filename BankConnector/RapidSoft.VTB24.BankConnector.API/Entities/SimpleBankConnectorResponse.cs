namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.VTB24.BankConnector.API.Exceptions;

    [DataContract]
    public class SimpleBankConnectorResponse
    {
        public SimpleBankConnectorResponse()
        {
            this.Success = true;
        }

        public SimpleBankConnectorResponse(Exception ex)
        {
            if (ex == null)
            {
                this.Success = true;
            }
            else
            {
                this.Success = false;
                this.ResultCode = (int)ExceptionTypeFactory.GetExceptionType(ex);
                this.Error = ex.Message;
            }
        }

        public SimpleBankConnectorResponse(int resultCode, bool success, string errorDescription = null)
        {
            this.ResultCode = resultCode;
            this.Success = success;
            this.Error = errorDescription;
        }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}
