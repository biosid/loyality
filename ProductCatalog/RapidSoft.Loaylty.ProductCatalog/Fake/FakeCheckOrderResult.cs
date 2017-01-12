namespace RapidSoft.Loaylty.ProductCatalog
{
    using System.Configuration;

    public class FakeCheckOrderResult : ConfigurationElement
    {
        [ConfigurationProperty("useFake", DefaultValue = false)]
        public bool UseFake
        {
            get { return (bool)base["useFake"]; }
            set { base["useFake"] = value; }
        }

        [ConfigurationProperty("checked")]
        public int Checked
        {
            get { return (int)base["checked"]; }
            set { base["checked"] = value; }
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