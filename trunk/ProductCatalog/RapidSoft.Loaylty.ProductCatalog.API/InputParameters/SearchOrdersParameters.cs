namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;

    using Entities;

    public class SearchOrdersParameters
    {
        public string UserId { get; set; }

        /// <summary>
        /// Минимальная дата и время создания заказа.
        /// </summary>
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Максимальная дата и время создания заказа.
        /// </summary>
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// Cтатус искомых заказов.
        /// </summary>
        public OrderStatuses[] Statuses
        {
            get;
            set;
        }

        /// <summary>
        /// Cтатус заказов которые следует исключить.
        /// </summary>
        public OrderStatuses[] SkipStatuses
        {
            get;
            set;
        }

        /// <summary>
        /// Cтатус оплаты заказов
        /// </summary>
        public OrderPaymentStatuses[] OrderPaymentStatuses
        {
            get;
            set;
        }

        /// <summary>
        /// Cтатус доставки оплаченых заказов
        /// </summary>
        public OrderDeliveryPaymentStatus[] OrderDeliveryPaymentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификаторы партнеров
        /// </summary>
        public int[] PartnerIds
        {
            get;
            set;
        }

        /// <summary>
        /// Количество пропущенных записей.
        /// </summary>
        public int? CountToSkip
        {
            get;
            set;
        }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        public int? CountToTake
        {
            get;
            set;
        }

        /// <summary>
        /// Признак подсчета общего количества найденных записей.
        /// </summary>
        public bool? CalcTotalCount
        {
            get;
            set;
        }

        public int[] OrderIds
        {
            get;
            set;
        }

        public int[] CarrierIds
        {
            get;
            set;
        }
    }
}