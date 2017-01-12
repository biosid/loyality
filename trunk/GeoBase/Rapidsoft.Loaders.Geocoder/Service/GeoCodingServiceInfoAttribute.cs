using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Service
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GeoCodingServiceInfoAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
