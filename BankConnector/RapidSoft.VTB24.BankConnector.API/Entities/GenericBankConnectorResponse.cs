using System;
using System.Runtime.Serialization;

namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    [DataContract]
    public class GenericBankConnectorResponse<T> : SimpleBankConnectorResponse
    {
        [DataMember]
        public T Result { get; set; }

        public GenericBankConnectorResponse(T result)
        {
            this.Result = result;
        }

        public GenericBankConnectorResponse(Exception ex) : base (ex)
        {
        }

        public GenericBankConnectorResponse(int resultCode, bool success, string errorDescription = null)
            : base(resultCode, success, errorDescription)
        {
        }
    }
}
