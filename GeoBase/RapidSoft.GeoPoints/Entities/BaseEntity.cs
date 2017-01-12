using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    [DataContract]
    public class BaseEntity
    {
        /// <summary>
        /// Дата и время вставки записи во временной зоне сервера
        /// </summary>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Дата и время вставки сущности во временной зоне UTC
        /// </summary>
        [DataMember]
        public DateTime CreatedUtcDateTime { get; set; }

        /// <summary>
        /// Дата и время обновления записи во временной зоне сервера
        /// </summary>
        [DataMember]
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// Дата и время обновления сущности во временной зоне UTC
        /// </summary>
        [DataMember]
        public DateTime ModifiedUtcDateTime { get; set; }
    }
}
