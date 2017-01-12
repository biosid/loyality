namespace RapidSoft.Loaylty.ProductCatalog
{
    using System.Configuration;

    public class FakeCommitOrdersResult : ConfigurationElement
    {
        [ConfigurationProperty("useFake", DefaultValue = false)]
        public bool UseFake
        {
            get { return (bool)base["useFake"]; }
            set { base["useFake"] = value; }
        }

        [ConfigurationProperty("success")]
        public bool Success
        {
            get { return (bool)base["success"]; }
            set { base["success"] = value; }
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