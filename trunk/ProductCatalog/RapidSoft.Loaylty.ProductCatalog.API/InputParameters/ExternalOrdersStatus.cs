namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ExternalOrdersStatus : OrdersStatus
    {
        /// <summary>
        /// Идентификатор заказа в информационной системе поставщика
        /// </summary>
        [DataMember]
        public string ExternalOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор партнёра.
        /// </summary>
        [DataMember]
        public int? PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Дата и время изменения статуса в информационной системе поставщика.
        /// </summary>
        [DataMember]
        public DateTime? ExternalOrderStatusDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Код статуса в информационной системе поставщика. Предназначен для сверок. Не обязателен к заполнению.
        /// </summary>
        [DataMember]
        public string ExternalOrderStatusCode
        {
            get;
            set;
        }
    }
}