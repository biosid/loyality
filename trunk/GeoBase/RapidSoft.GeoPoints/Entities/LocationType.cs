using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Тип местоположения
    /// </summary>
    public class LocationType : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
