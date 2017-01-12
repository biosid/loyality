namespace RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper.Configuration
{
    using System.Configuration;

    public class EtlVariableElement : ConfigurationElement
    {
        private const string NameProperty = "name";

        private const string ValueProperty = "value";

        [ConfigurationProperty(NameProperty, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameProperty]; }
        }

        [ConfigurationProperty(ValueProperty, IsRequired = false, IsKey = false)]
        public string Value
        {
            get
            {
                return (string)this[ValueProperty];
            }
        }
    }
}
