using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Точка обслуживания
    /// </summary>
    [DataContract]
    public class ServicePoint : Location
    {
        /// <summary>
        /// Код
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Идентификатор системы моментальных переводов (ее ExternalId)
        /// </summary>
        [DataMember]
        public string InstantTransferSystem { get; set; }

        /// <summary>
        /// Признак точки для безадресных переводов
        /// </summary>
        [DataMember]
        public int? Unaddressed { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Режим работы
        /// </summary>
        [DataMember]
        public string Schedule { get; set; }

        /// <summary>
        /// Список валют
        /// </summary>
        [DataMember]
        public string Currency { get; set; }

        /// <summary>
        /// Минимальная сумма
        /// </summary>
        [DataMember]
        public string Summa { get; set; }

        /// <summary>
        /// Максимальная сумма
        /// </summary>
        [DataMember]
        public string MaxSumma { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}