using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.IPDB.Entities
{
    public class IpDbRecord
    {
        public long IPFromNumber { get; set; }
        public long IPToNumber { get; set; }

        public string IPFrom { get; set; }
        public string IPTo { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string FedRegion { get; set; }

    }
}
