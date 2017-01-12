namespace RapidSoft.VTB24.BankConnector.Configuration
{
    using System;
    using System.Configuration;

    public class EtlConfigSection : ConfigurationSection
    {
        private const string SectionName = "etlConfig";

        private const string ParamsPropName = "EtlVariables";

        public static EtlConfigSection Current
        {
            get
            {
                var section = (EtlConfigSection)ConfigurationManager.GetSection(SectionName);
                return section;
            }
        }

        [ConfigurationProperty(ParamsPropName, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ParamElement),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ConfigurationElementCollection<ParamElement> Variables
        {
            get
            {
                return (ConfigurationElementCollection<ParamElement>)base[ParamsPropName];
            }
        }

        public static string GetEtlVarOrDefault(string varName)
        {
            if (Current == null)
            {
                return null;
            }

            var etlVar = Current.Variables[varName];

            if (etlVar == null)
            {
                return null;
            }

            return etlVar.Value;            
        }

        public static string GetEtlVar(string varName)
        {
            if (Current == null)
            {
                throw new InvalidOperationException(string.Format("{0} section not set", SectionName));
            }

            var etlVar = Current.Variables[varName];

            if (etlVar == null)
            {
                throw new InvalidOperationException(string.Format("etlVar {0} not set in section {1}", varName, SectionName));                
            }

            return etlVar.Value;
        }
    }
}
