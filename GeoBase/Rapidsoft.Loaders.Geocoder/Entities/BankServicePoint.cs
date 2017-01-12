using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Entities
{
    public class BankServicePoint : ILocation
    {
        public object Id { get; set; }
        public string City { get; set; }
        public string SourceAddress { get; set; }
        public string Address { get; set; }
    }
}
