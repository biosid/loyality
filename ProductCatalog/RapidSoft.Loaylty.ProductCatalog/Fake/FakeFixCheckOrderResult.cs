namespace RapidSoft.Loaylty.ProductCatalog.Fake
{
    using System.Configuration;

    public class FakeFixBasketItemPriceResult : ConfigurationElement
    {
        [ConfigurationProperty("useFake", DefaultValue = false)]
        public bool UseFake
        {
            get { return (bool)base["useFake"]; }
            set { base["useFake"] = value; }
        }

        [ConfigurationProperty("confirmed")]
        public int Confirmed
        {
            get { return (int)base["confirmed"]; }
            set { base["confirmed"] = value; }
        }

        [ConfigurationProperty("actualPrice")]
        public int ActualPrice
        {
            get { return (int)base["actualPrice"]; }
            set { base["actualPrice"] = value; }
        }

        [ConfigurationProperty("reason")]
        public string Reason
        {
            get { return (string)base["reason"]; }
            set { base["reason"] = value; }
        }

        [ConfigurationProperty("reasonCode")]
        public string ReasonCode
        {
            get { return (string)base["reasonCode"]; }
            set { base["reasonCode"] = value; }
        }
    }
}