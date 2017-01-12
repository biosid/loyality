namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    [ServiceContract]
    public interface IPartnerSecurityService : ISupportService
    {
        [OperationContract]
        SignatureResult CreateOnlinePartnerSignature(int partnerId, string text);

        [OperationContract]
        SignatureResult CreateBankSignature(string text);

        [OperationContract]
        VerifyResult VerifyBankSignature(string checkValue, string signature);

        [OperationContract]
        VerifyResult VerifyOnlinePartnerSignature(string checkValue, string signature, int partnerId);
    }
}
