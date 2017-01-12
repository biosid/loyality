namespace RapidSoft.VTB24.BankConnector.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T>
        where T : ConfigurationElement, IKeyProvider, new()
    {
        public T this[string key]
        {
            get
            {
                return (T)this.BaseGet(key);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return (T)this.BaseGet(i);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).GetKey();
        }
    }
}