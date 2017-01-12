namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class GetOrderStatusesHistoryParameters
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Количество пропущенных записей.
        /// </summary>
        public int CountToSkip { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        public int CountToTake { get; set; }

        /// <summary>
        /// Признак подсчета общего количества найденных записей.
        /// </summary>
        public bool CalcTotalCount { get; set; }

        public string UserId { get; set; }
    }
}