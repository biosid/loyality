using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Диапазон IP-адресов
    /// </summary>
    public class IPLocation : BaseEtlEntity
    {
        /// <summary>
        /// Идентификатор диапазона
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Начальный IP-адрес диапазона в десятичной форме
        /// </summary>
        public long IPV4From { get; set; }

        /// <summary>
        /// Конечный IP-адрес диапазона в десятичной форме
        /// </summary>
        public long IPV4To { get; set; }

        /// <summary>
        /// Начальный IP-адрес диапазона в десятичной форме с точками
        /// </summary>
        public string IPV4FromString { get; set; }

        /// <summary>
        /// Конечный IP-адрес диапазона в десятичной форме с точками
        /// </summary>
        public string IPV4ToString { get; set; }

        /// <summary>
        /// Наименование компании, для которой действует подсеть
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Идентификатор местоположения, для которого действует диапазон
        /// </summary>
        public Guid LocationId { get; set; }
    }
}
