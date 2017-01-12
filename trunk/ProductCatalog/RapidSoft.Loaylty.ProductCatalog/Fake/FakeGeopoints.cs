namespace RapidSoft.Loaylty.ProductCatalog.Fake
{
    using System.Configuration;

    public class FakeGeopoints : ConfigurationElement
    {
        [ConfigurationProperty("useFake", DefaultValue = false)]
        public bool UseFake
        {
            get { return (bool)base["useFake"]; }
            set { base["useFake"] = value; }
        }
    }
}