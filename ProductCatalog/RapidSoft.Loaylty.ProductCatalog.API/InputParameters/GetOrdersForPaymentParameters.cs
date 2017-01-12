namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class GetOrdersForPaymentParameters
    {
        /// <summary>
        /// Количество пропущенных записей.
        /// </summary>
        public int CountToSkip { get; set; }

        /// <summary>
        /// Максимальное количество возвращаемых записей.
        /// </summary>
        public int CountToTake { get; set; }
    }
}