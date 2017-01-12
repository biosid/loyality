namespace RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper.Configuration
{
    using System.Configuration;

    public class EtlConfigSection : ConfigurationSection
    {
        private const string SectionName = "etl_config";

        private const string VariablesProperty = "variables";

        public static EtlConfigSection Current
        {
            get
            {
                var section = (EtlConfigSection)ConfigurationManager.GetSection(SectionName);
                return section;
            }
        }

        [ConfigurationProperty(VariablesProperty, IsDefaultCollection = false)]
        public EtlVariablesCollection Variables
        {
            get
            {
                return (EtlVariablesCollection)base[VariablesProperty];
            }
        }
    }
}
