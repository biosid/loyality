namespace RapidSoft.GeoPoints.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Местоположение
    /// </summary>
    [DataContract]
    [KnownType(typeof(ServicePoint))]
    public class Location
    {
        /// <summary>
        /// Идентификатор местоположения
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор родительской локации
        /// </summary>
        [DataMember]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Идентификатор местоположения из внешней системы
        /// </summary>
        [DataMember]
        public string ExternalId { get; set; }

        /// <summary>
        /// Тип местоположения
        /// </summary>
        [DataMember]
        public int LocationType { get; set; }

        /// <summary>
        /// Название местоположения
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Топоним
        /// </summary>
        [DataMember]
        public string Toponym { get; set; }

        /// <summary>
        /// Код по КЛАДР ФНС РФ
        /// </summary>
        [DataMember]
        public string KladrCode { get; set; }

        /// <summary>
        /// Индекс
        /// </summary>
        [DataMember]
        public string Index { get; set; }

        /// <summary>
        /// Идентификатор страны
        /// </summary>
        [DataMember]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        [DataMember]
        public string RegionName { get; set; }

        /// <summary>
        /// Идентификатор региона
        /// </summary>
        [DataMember]
        public Guid? RegionId { get; set; }

        /// <summary>
        /// Топоним региона
        /// </summary>
        [DataMember]
        public string RegionToponym { get; set; }

        [DataMember]
        public string RegionKladrCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.KladrCode))
                {
                    return this.KladrCode;
                }

                return this.KladrCode.Substring(0, 2) + "00000000000";
            }
            
            // ReSharper disable ValueParameterNotUsed
            set
            {
            }
            // ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Наименование района
        /// </summary>
        [DataMember]
        public string DistrictName { get; set; }

        /// <summary>
        /// Идентификатор района
        /// </summary>
        [DataMember]
        public Guid? DistrictId { get; set; }

        /// <summary>
        /// Топоним района
        /// </summary>
        [DataMember]
        public string DistrictToponym { get; set; }

        [DataMember]
        public string DistrictKladrCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.KladrCode))
                {
                    return this.KladrCode;
                }

                if (this.DistrictId == null)
                {
                    return null;
                }

                return this.KladrCode.Substring(0, 5) + "00000000";
            }

            // ReSharper disable ValueParameterNotUsed
            set
            {
            }
            // ReSharper restore ValueParameterNotUsed
        }
        
        /// <summary>
        /// Наименование города
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        [DataMember]
        public Guid? CityId { get; set; }

        /// <summary>
        /// Топоним города
        /// </summary>
        [DataMember]
        public string CityToponym { get; set; }

        [DataMember]
        public string CityKladrCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.KladrCode))
                {
                    return this.KladrCode;
                }

                if (this.CityId == null)
                {
                    // NOTE: Москва и Санкт-Петербург особый случай
                    if (this.KladrCode == "7700000000000" || this.KladrCode == "7800000000000")
                    {
                        return this.KladrCode;
                    }

                    return null;
                }

                return this.KladrCode.Substring(0, 8) + "00000";
            }

            // ReSharper disable ValueParameterNotUsed
            set
            {
            }
            // ReSharper restore ValueParameterNotUsed
        }
        
        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        [DataMember]
        public string TownName { get; set; }

        /// <summary>
        /// Идентификатор населенного пункта
        /// </summary>
        [DataMember]
        public Guid? TownId { get; set; }

        /// <summary>
        /// Топоним населенного пункта
        /// </summary>
        [DataMember]
        public string TownToponym { get; set; }

        [DataMember]
        public string TownKladrCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.KladrCode))
                {
                    return this.KladrCode;
                }

                if (this.TownId == null)
                {
                    return null;
                }

                return this.KladrCode.Substring(0, 11) + "00";
            }

            // ReSharper disable ValueParameterNotUsed
            set
            {
            }
            // ReSharper restore ValueParameterNotUsed
        }
        
        /// <summary>
        /// Адрес местоположения в текстовом виде
        /// </summary>
        [DataMember]
        public string Address { get; set; }
    }
}
