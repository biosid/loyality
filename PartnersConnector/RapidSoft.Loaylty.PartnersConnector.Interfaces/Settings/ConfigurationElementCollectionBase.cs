namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings
{
	using System;
	using System.Configuration;

	public abstract class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMapAlternate;
			}
		}

		protected override string ElementName
		{
			get { return this.GetElementName(); }
		}

		public T this[int idx]
		{
			get
			{
				return (T)this.BaseGet(idx);
			}
		}

		protected abstract string GetElementName();

		protected abstract object GetElementKey(T element);

		protected override bool IsElementName(string elementName)
		{
			return elementName.Equals(this.GetElementName(), StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool IsReadOnly()
		{
			return false;
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return this.GetElementKey((T)element);
		}
	}
}