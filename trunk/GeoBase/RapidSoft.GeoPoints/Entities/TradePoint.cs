using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Точка обслуживания
    /// </summary>
    public class TradePoint : BaseEtlEntity
    {
        /// <summary>
        /// Идентификатор местоположения
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Идентификатор точки обслуживания из внешней системы
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Наименование точки обслуживания
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание точки обслуживания
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип точки обслуживания
        /// </summary>
        public Guid TypeId { get; set; }
    }
}
