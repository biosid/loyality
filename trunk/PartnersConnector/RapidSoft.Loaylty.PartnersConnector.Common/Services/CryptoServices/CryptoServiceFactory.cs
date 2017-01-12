namespace RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices
{
    using System;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;

    /// <summary>
    /// Фабрика крипто-сервисов.
    /// </summary>
    public class CryptoServiceFactory : ICryptoServiceFactory
    {
        private static readonly object LockObj = new object();
        private static ICryptoService bankCryptoService;
        private static ICryptoService partnerCryptoService;

        private readonly ICatalogAdminServiceProvider catalogAdminServiceProvider;

        public CryptoServiceFactory(ICatalogAdminServiceProvider catalogAdminServiceProvider = null)
        {
            this.catalogAdminServiceProvider = catalogAdminServiceProvider ?? new CatalogAdminServiceProvider();
        }

        public ICryptoService GetBankCryptoService()
        {
            if (bankCryptoService != null)
            {
                return bankCryptoService;
            }

            var privateKey = PartnerConnectionsConfig.BankPrivateKey;

            if (string.IsNullOrWhiteSpace(privateKey))
            {
               throw new Exception("Не задан private ключ банка.");
            }

            lock (LockObj)
            {
                if (bankCryptoService != null)
                {
                    return bankCryptoService;
                }

                bankCryptoService = new PemPrivateKeyCryptoService(privateKey);
                return bankCryptoService;
            }
        }

        public ICryptoService GetParnterCryptoService(int partnerId)
        {
            var section = this.catalogAdminServiceProvider.GetPartnerSettings(partnerId);

            if (section == null || string.IsNullOrWhiteSpace(section.PublicKey))
            {
                var mess = string.Format("Не задан public ключ для партнера {0}.", partnerId);
                throw new Exception(mess);
            }

            var publicKey = section.PublicKey;

            var partnerCryptoService = new PemPublicKeyCryptoService(publicKey);
            return partnerCryptoService;
        }

        public ICryptoService GetPartnerCryptoService(int partnerId)
        {
            if (partnerCryptoService != null)
            {
                return partnerCryptoService;
            }

            var section = this.catalogAdminServiceProvider.GetPartnerSettings(partnerId);

            if (section == null || string.IsNullOrWhiteSpace(section.PrivateKey))
            {
                var mess = string.Format("Не задан private ключ для партнера {0}.", partnerId);
                throw new Exception(mess);
            }

            var privateKey = section.PrivateKey;

            lock (LockObj)
            {
                if (partnerCryptoService != null)
                {
                    return partnerCryptoService;
                }

                partnerCryptoService = new PemPrivateKeyCryptoService(privateKey);
                return partnerCryptoService;
            }

        }
    }
}