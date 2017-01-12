namespace RapidSoft.Loaylty.PartnersConnector.Services
{
    using System;

    using Logging;

    using Monitoring;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;

    public class PartnerSecurityService : SupportService, IPartnerSecurityService
    {
        private readonly ILog log = LogManager.GetLogger(typeof (PartnerSecurityService));

        public SignatureResult CreateOnlinePartnerSignature(int partnerId, string text)
        {
            try
            {
                var factory = new CryptoServiceFactory();
                var cryptoService = factory.GetPartnerCryptoService(partnerId);
                var sign = cryptoService.CreateSignature(text);
                return SignatureResult.BuildSuccess(sign);
            }
            catch (Exception ex)
            {
                var mess = string.Format("Ошибка получения подписи: {0}", ex.Message);
                log.ErrorFormat(mess, ex);
                return SignatureResult.BuildFail(mess);
            }
        }

        public SignatureResult CreateBankSignature(string text)
        {
            try
            {
                var factory = new CryptoServiceFactory();
                var bankCryptoService = factory.GetBankCryptoService();
                var sign = bankCryptoService.CreateSignature(text);
                return SignatureResult.BuildSuccess(sign);
            }
            catch (Exception ex)
            {
                var mess = string.Format("Ошибка получения подписи: {0}", ex.Message);
                log.ErrorFormat(mess, ex);
                return SignatureResult.BuildFail(mess);
            }
        }

        public VerifyResult VerifyBankSignature(string checkValue, string signature)
        {
            try
            {
                var factory = new CryptoServiceFactory();
                var bankCryptoService = factory.GetBankCryptoService();
                var isValid = bankCryptoService.VerifyData(signature, checkValue);
                return VerifyResult.BuildSuccess(isValid);
            }
            catch (Exception ex)
            {
                var mess = string.Format("Ошибка проверки подписи: {0}", ex.Message);
                log.ErrorFormat(mess, ex);
                return VerifyResult.BuildFail(mess);
            }
        }

        public VerifyResult VerifyOnlinePartnerSignature(string checkValue, string signature, int partnerId)
        {
            try
            {
                var factory = new CryptoServiceFactory();
                var parnterCryptoService = factory.GetParnterCryptoService(partnerId);
                var isValid = parnterCryptoService.VerifyData(signature, checkValue);
                return VerifyResult.BuildSuccess(isValid);
            }
            catch (Exception ex)
            {
                var mess = string.Format("Ошибка проверки подписи: {0}", ex.Message);
                log.ErrorFormat(mess, ex);
                return VerifyResult.BuildFail(mess);
            }
        }
    }
}
