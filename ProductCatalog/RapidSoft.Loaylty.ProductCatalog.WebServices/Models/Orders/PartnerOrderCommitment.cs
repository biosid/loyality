namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PartnerOrderCommitment
    {
        /// <summary>
        /// Уникальный идентификатор заказа в системе лояльности
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор, присвоенный заказу информационной системой поставщика. Если такой идентификатор отсутствует, то указывается переданный идентификатор заказа на сайте Коллекции. 
        /// </summary>
        [DataMember]
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Факт подтверждения заказа. Принимает одно из следующих значений:
        /// false – заказ не был подтвержден поставщиком (отказ);
        /// true – заказ был подтвержден и принят на выполнение поставщиком.
        /// </summary>
        [DataMember]
        public bool IsConfirmed { get; set; }

        /// <summary>
        /// Текстовое описание причины отказа. Описание предназначено для отображения пользователю. Заполняется, если Confirmed = 0.
        /// </summary>
        [DataMember]
        public string Reason { get; set; }

        /// <summary>
        /// Код причины отказа в информационной системе поставщика. Код предназначен для логирования, сбора статистики и разбора. Не обязателен к заполнению.
        /// </summary>
        [DataMember]
        public string ReasonCode { get; set; }
    }
}
