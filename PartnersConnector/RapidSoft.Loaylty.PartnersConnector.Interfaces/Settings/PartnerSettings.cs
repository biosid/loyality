namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings
{
    using System.Collections.Generic;

    public class PartnerSettings
    {
        private readonly Dictionary<string, string> settings;

        public PartnerSettings(int partnerId, Dictionary<string, string> settings)
        {
            this.PartnerId = partnerId;
            this.settings = settings;
        }

        public int PartnerId { get; private set; }

        public string GetDeliveryVariants
        {
            get { return this.settings["GetDeliveryVariants"]; }
        }

        public string FixBasketItemPrice
        {
            get { return this.settings["FixBasketItemPrice"]; }
        }

        public string OrderCheck
        {
            get { return this.settings["Check"]; }
        }

        public string OrderConfirmation
        {
            get { return this.settings["Confirmation"]; }
        }

        public string BatchOrderConfirmation
        {
            get { return this.settings["BatchConfirmation"]; }
        }

        public string CertificateThumbprint
        {
            get { return this.settings["CertificateThumbprint"]; }
        }

        public bool UseBatch
        {
            get
            {
                bool retVal;
                string useBatchStr;

                return this.settings.TryGetValue("UseBatch", out useBatchStr) &&
                       bool.TryParse(useBatchStr, out retVal) &&
                       retVal;
            }
        }

        public string PublicKey
        {
            get { return this.settings["PublicKey"]; }
        }

        public string PrivateKey
        {
            get { return this.settings["PrivateKey"]; }
        }
    }
}