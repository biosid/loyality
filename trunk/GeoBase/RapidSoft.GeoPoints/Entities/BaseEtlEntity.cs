using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    [DataContract]
    public class BaseEtlEntity : BaseEntity
    {
        [DataMember]
        public Guid EtlPackageId { get; set; }

        [DataMember]
        public Guid EtlSessionId { get; set; }
    }
}
