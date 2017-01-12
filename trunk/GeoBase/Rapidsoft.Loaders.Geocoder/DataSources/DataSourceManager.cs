using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.DataSources
{
    public class DataSourceManager
    {
        public static IDataSource GetDataSource(string name)
        {
            foreach (var type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
                if (typeof(IDataSource) != type && typeof(IDataSource).IsAssignableFrom(type))
                {
                    var attributes = type.GetCustomAttributes(typeof(DataSourceInfoAttribute), false);
                    if (attributes.Count() != 1)
                        throw new Exception(String.Format("Missing DataSourceInfo attribute definition for type {0}", type.Name));

                    if (((DataSourceInfoAttribute)attributes.Single()).Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return (IDataSource)Activator.CreateInstance(type);
                }

            return null;
        }
    }
}
