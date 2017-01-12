using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.DataSources
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataSourceInfoAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
