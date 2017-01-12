namespace RapidSoft.Loaylty.ProductCatalog.Fake
{
    using System.Configuration;

    public class FakeCalcSingleValMechanic : ConfigurationElement
    {
        [ConfigurationProperty("useFake", DefaultValue = false)]
        public bool UseFake
        {
            get { return (bool)base["useFake"]; }
            set { base["useFake"] = value; }
        }

        [ConfigurationProperty("result")]
        public int Result
        {
            get { return (int)base["result"]; }
            set { base["result"] = value; }
        }
    }
}