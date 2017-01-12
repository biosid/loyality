namespace RapidSoft.Loaylty.ProductCatalog.Fake
{
    using System.Configuration;

    public class FakeMechanic : ConfigurationElement
    {
        [ConfigurationProperty("useFakePriceMechanic", DefaultValue = false)]
        public bool UseFakePriceMechanic
        {
            get { return (bool)base["useFakePriceMechanic"]; }
            set { base["useFakePriceMechanic"] = value; }
        }

        [ConfigurationProperty("baseMult")]
        public string BaseMult
        {
            get { return (string)base["baseMult"]; }
            set { base["baseMult"] = value; }
        }

        [ConfigurationProperty("baseAdd")]
        public string BaseAdd
        {
            get { return (string)base["baseAdd"]; }
            set { base["baseAdd"] = value; }
        }

        [ConfigurationProperty("mult")]
        public string Mult
        {
            get { return (string)base["mult"]; }
            set { base["mult"] = value; }
        }

        [ConfigurationProperty("add")]
        public string Add
        {
            get { return (string)base["add"]; }
            set { base["add"] = value; }
        }
    }
}