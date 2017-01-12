namespace RapidSoft.VTB24.BankConnector.Configuration
{
    using System.Configuration;

    public class ParamElement : ConfigurationElement, IKeyProvider
    {
        private const string NamePropName = "name";

        private const string ValuePropName = "value";

        #region Properties

        [ConfigurationProperty(NamePropName, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NamePropName]; }
        }

        [ConfigurationProperty(ValuePropName, IsRequired = false, IsKey = false)]
        public string Value
        {
            get
            {
                return (string)this[ValuePropName];
            }
        }

        #endregion

        #region IKeyProvider Members

        public object GetKey()
        {
            return this.Name;
        }

        #endregion
    }
}