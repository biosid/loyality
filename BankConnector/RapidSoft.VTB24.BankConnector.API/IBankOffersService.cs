using System.ServiceModel;
using RapidSoft.VTB24.BankConnector.API.Entities;

namespace RapidSoft.VTB24.BankConnector.API
{
    [ServiceContract]
    public interface IBankOffersService
    {
        [OperationContract]
        GenericBankConnectorResponse<BankOffersServiceResponse> GetBankOffers(BankOffersServiceParameter parameter);

        [OperationContract]
        SimpleBankConnectorResponse DisableOffer(string offerId);
    }
}
