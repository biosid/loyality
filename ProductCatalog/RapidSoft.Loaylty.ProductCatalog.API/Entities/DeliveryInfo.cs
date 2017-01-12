using System;
using System.Runtime.Serialization;

namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{

    /// <summary>
    /// Сведения о доставке
    /// </summary>
    [DataContract]
    public class DeliveryInfo
    {
        #region Properties

        /// <summary>
        /// Местоположение использовавшееся для получения вариантов доставки
        /// </summary>
        [DataMember]
        public VariantsLocation DeliveryVariantsLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Выбранный тип доставки
        /// </summary>
        [DataMember]
        public DeliveryTypes DeliveryType { get; set; }

        /// <summary>
        /// Идентификатор варианта доставки полученный от партнёра
        /// </summary>
        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        /// <summary>
        /// Наименование выбранного способа доставки
        /// </summary>
        [DataMember]
        public string DeliveryVariantName { get; set; }

        /// <summary>
        /// Наименование выбранного способа доставки
        /// </summary>
        [DataMember]
        public string DeliveryVariantDescription { get; set; }

        /// <summary>
        /// Адресс доставки обязателен для DeliveryType.Delivery
        /// </summary>
        [DataMember]
        public DeliveryAddress Address { get; set; }

        /// <summary>
        /// Адресс самовывоза обязательна для DeliveryType.PickUp
        /// </summary>
        [DataMember]
        public PickupPoint PickupPoint { get; set; }

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
        /// Текст, который необходимо передать получателю вознаграждения. 
        /// Если поставщик не поддерживает данную опцию, то ее необходимо игнорировать при подтверждении заказа.
        /// </summary>
        [DataMember]
        public string AddText { get; set; }

        /// <summary>
        /// Контактная информация
        /// </summary>
        [DataMember]
        public Contact Contact { get; set; }

        /// <summary>
        /// Комментарии клиента для службы доставки
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        #endregion
    }
}