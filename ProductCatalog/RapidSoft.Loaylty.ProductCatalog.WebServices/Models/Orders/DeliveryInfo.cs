namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Сведения о доставке
    /// </summary>
    [DataContract]
    public class DeliveryInfo
    {
        #region Properties

        /// <summary>
        /// Код страны по ОКСМ, куда должна быть осуществлена доставка.
        /// </summary>
        [DataMember]
        public string CountryCode { get; set; }

        /// <summary>
        /// Краткое наименование страны по ОКСМ, куда должна быть осуществлена доставка.
        /// </summary>
        [DataMember]
        public string CountryName { get; set; }

        /// <summary>
        /// Почтовый индекс адреса доставки.
        /// </summary>
        [DataMember]
        public string PostCode { get; set; }

        /// <summary>
        /// КЛАДР код адреса без улицы.
        /// </summary>
        [DataMember]
        public string AddressKladrCode { get; set; }

        /// <summary>
        /// Кладр код региона доставки.
        /// </summary>
        [DataMember]
        public string RegionKladrCode
        {
            get;
            set;
        }

        /// <summary>
        /// Кладр код района доставки.
        /// </summary>
        [DataMember]
        public string DistrictKladrCode
        {
            get;
            set;
        }

        /// <summary>
        /// Кладр код города доставки.
        /// </summary>
        [DataMember]
        public string CityKladrCode
        {
            get;
            set;
        }

        /// <summary>
        /// Кладр код населённого пункта доставки.
        /// </summary>
        [DataMember]
        public string TownKladrCode
        {
            get;
            set;
        }

        /// <summary>
        /// Наименование региона доставки в свободной форме.
        /// </summary>
        [DataMember]
        public string RegionTitle { get; set; }

        /// <summary>
        /// Наименование района доставки в свободной форме.
        /// </summary>
        [DataMember]
        public string DistrictTitle { get; set; }

        /// <summary>
        /// Наименование города доставки в свободной форме.
        /// </summary>
        [DataMember]
        public string CityTitle { get; set; }

        /// <summary>
        /// Наименование населённого пункта доставки в свободной форме.
        /// </summary>
        [DataMember]
        public string TownTitle { get; set; }

        /// <summary>
        /// Наименование улицы доставки в свободной форме.
        /// </summary>
        [DataMember]
        public string StreetTitle { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        [DataMember]
        public string House { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        [DataMember]
        public string Flat { get; set; }

        /// <summary>
        /// Текстовое представление адреса.
        /// </summary>
        [DataMember]
        public string AddressText { get; set; }

        /// <summary>
        /// Желаемая дата доставки
        /// </summary>
        [DataMember]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Желаемое время начала доставки
        /// </summary>
        [DataMember]
        public TimeSpan? DeliveryTimeFrom { get; set; }

        /// <summary>
        /// Желаемое время окончания доставки
        /// </summary>
        [DataMember]
        public TimeSpan? DeliveryTimeTo { get; set; }

        /// <summary>
        /// Комментарии клиента для службы доставки
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// Текст, который необходимо передать получателю вознаграждения. 
        /// Если поставщик не поддерживает данную опцию, то ее необходимо игнорировать при подтверждении заказа.
        /// </summary>
        [DataMember]
        public string AdditionalText { get; set; }

        /// <summary>
        /// Контактная информация
        /// </summary>
        [DataMember]
        public Contact[] Contacts { get; set; }

        #endregion
    }
}