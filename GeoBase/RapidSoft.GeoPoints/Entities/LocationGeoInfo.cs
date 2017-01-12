using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Информация о геокодировании местоположения
    /// </summary>
    public class LocationGeoInfo : BaseEtlEntity
    {
        /// <summary>
        /// Идентификатор местоположения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование системы геокодирования
        /// </summary>
        public string GeoSystem { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public decimal Lat { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public decimal Lng { get; set; }

        /// <summary>
        /// Статус геокодирования
        /// </summary>
        public GeoCodingStatus GeoCodingStatus { get; set; }

        /// <summary>
        /// Точность геокодирования
        /// </summary>
        public GeoCodingAccuracy GeoCodingAccuracy { get; set; }

        /// <summary>
        /// Дата и время геокодирования
        /// </summary>
        public DateTime GeoDateTime { get; set; }
    }
}
