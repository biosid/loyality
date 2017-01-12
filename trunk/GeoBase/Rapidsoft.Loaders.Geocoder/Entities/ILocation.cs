using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Entities
{
    public interface ILocation
    {
        object Id { get; set; }
        string Address { get; set; }
    }
}
