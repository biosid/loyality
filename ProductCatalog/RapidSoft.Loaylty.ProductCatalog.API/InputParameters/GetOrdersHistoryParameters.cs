namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetOrdersHistoryParameters
    {
        public GetOrdersHistoryParameters()
        {
        }

        public GetOrdersHistoryParameters(
            string clientId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            PublicOrderStatuses[] status = null,
            int? countToSkip = null, 
            int? countToTake = null, 
            bool? calcTotalCount = null)
        {
            this.ClientId = clientId;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Statuses = status;
            this.CountToSkip = countToSkip;
            this.CountToTake = countToTake;
            this.CalcTotalCount = calcTotalCount;
        }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Минимальная дата и время создания заказа.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Максимальная дата и время создания заказа.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Публичный статус искомых заказов.
        /// </summary>
        public PublicOrderStatuses[] Statuses { get; set; }

        /// <summary>
        /// Количество пропущенных записей.
        /// </summary>
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        public int? CountToTake { get; set; }

        /// <summary>
        /// Признак подсчета общего количества найденных записей.
        /// </summary>
        public bool? CalcTotalCount { get; set; }
    }
}