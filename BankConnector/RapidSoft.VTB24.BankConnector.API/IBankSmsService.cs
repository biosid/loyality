using System.ServiceModel;
using RapidSoft.VTB24.BankConnector.API.Entities;

namespace RapidSoft.VTB24.BankConnector.API
{
    [ServiceContract]
    public interface IBankSmsService
    {
        [OperationContract]
        SimpleBankConnectorResponse EnqueueSms(BankSmsType type, string phone, string password);
    }
}
