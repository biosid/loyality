namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class DeliveryDto
    {
        /// <summary>
        /// Выбранный тип доставки
        /// </summary>
        [DataMember]
        public DeliveryTypes DeliveryType { get; set; }

        [DataMember]
        public string ExternalDeliveryVariantId { get; set; }

        [DataMember]
        public Location DeliveryVariantLocation { get; set; }
        
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
        /// Комментарии клиента для службы доставки
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// Контактная информация
        /// </summary>
        [DataMember]
        public Contact Contact { get; set; }
    }
}