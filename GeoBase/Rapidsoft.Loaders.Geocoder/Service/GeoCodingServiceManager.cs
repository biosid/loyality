using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Service
{
    public class GeoCodingServiceManager
    {
        public static IGeocodingService GetGeocodingService(string name)
        {
            foreach (var type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
                if (typeof(IGeocodingService) != type && typeof(IGeocodingService).IsAssignableFrom(type))
                {
                    var attributes = type.GetCustomAttributes(typeof(GeoCodingServiceInfoAttribute), false);
                    if (attributes.Count() != 1)
                        throw new Exception(String.Format("Missing GeoCodingServiceInfo attribute definition for type {0}", type.Name));

                    if (((GeoCodingServiceInfoAttribute)attributes.Single()).Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return (IGeocodingService)Activator.CreateInstance(type);
                }

            return null;
        }
    }
}
