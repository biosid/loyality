namespace RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper.Configuration
{
    using System.Configuration;

    [ConfigurationCollection(typeof(EtlVariableElement), AddItemName = "variable")]
    public class EtlVariablesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EtlVariableElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EtlVariableElement)element).Name;
        }
    }
}
