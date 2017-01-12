namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    public class GetBasketParameters : IPagingParameters, IClientContextParameters
    {
        /// <summary>
        /// Количество возвращаемых элементов корзины. 
        /// Например, есть 5 элементов в корзине, а запрошено 10; 
        /// будет возвращено 5 элементов, но <see cref="CountToTake"/> будет содержать число 10.
        /// </summary>
        public int? CountToTake { get; set; }

        /// <summary>
        /// Количество пропускаемых элементов корзины
        /// </summary>
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Признак вычислять ли общее количество элементов корзины без учета <see cref="CountToSkip"/> и <see cref="CountToTake"/>.
        /// </summary>
        public bool? CalcTotalCount { get; set; }

        /// <summary>
        /// Контекст клиента.
        /// </summary>
        public Dictionary<string, string> ClientContext { get; set; }

        /// <summary>
        /// Идентификатор клиента в системе безопасности
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }
    }
}