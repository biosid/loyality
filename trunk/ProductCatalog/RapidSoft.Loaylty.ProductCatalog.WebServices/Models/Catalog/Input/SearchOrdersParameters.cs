namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;

    [DataContract]
    public class SearchOrdersParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Минимальная дата и время создания заказа.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Максимальная дата и время создания заказа.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Cтатус искомых заказов.
        /// </summary>
        public OrderStatuses[] Statuses { get; set; }

        /// <summary>
        /// Cтатус заказов которые следует исключить.
        /// </summary>
        public OrderStatuses[] SkipStatuses { get; set; }

        /// <summary>
        /// Cтатус оплаты заказов
        /// </summary>
        public PaymentStatuses[] OrderPaymentStatuses { get; set; }

        /// <summary>
        /// Cтатус доставки оплаченых заказов
        /// </summary>
        public PaymentStatuses[] OrderDeliveryPaymentStatus { get; set; }

        /// <summary>
        /// Идентификаторы партнеров
        /// </summary>
        public int[] PartnerIds { get; set; }

        public int[] OrderIds { get; set; }

        public int[] CarrierIds { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
