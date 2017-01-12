using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Локализация местоположения
    /// </summary>
    public class LocationLocalization : BaseEntity
    {
        /// <summary>
        /// Идентификатор местоположения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код языка по ISO 639-1
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Имя местоположения на языке, указанном в атрибуте Locale
        /// </summary>
        public decimal Name { get; set; }
    }
}
