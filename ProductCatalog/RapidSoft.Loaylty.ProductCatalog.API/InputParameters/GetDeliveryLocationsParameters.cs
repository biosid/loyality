namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetDeliveryLocationsParameters
    {
        public int PartnerId { get; set; }

        public DeliveryLocationStatus[] StatusFilters { get; set; }

        /// <summary>
        /// Указывает на необходимость возвращать только те точки доставки, которые:
        ///     true - имеют тариф доставки
        ///     false - НЕ имеют тариф доставки
        ///     null - и имеют и НЕ имеют тариф доставки
        /// </summary>
        public bool? HasRates { get; set; }

        public string SearchTerm { get; set; }

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