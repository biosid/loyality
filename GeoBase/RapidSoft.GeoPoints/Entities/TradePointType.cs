using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Тип точки обслуживания
    /// </summary>
    public class TradePointType : BaseEntity
    {
        /// <summary>
        /// Идентификатор типа точки обслуживания
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование типа точки обслуживания
        /// </summary>
        public string Name { get; set; }
    }
}
