using System.Collections.Generic;

namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class GetWishListParameters
    {
        /// <summary>
        /// Количество возвращаемых элементов. 
        /// Например, есть 5 элементов, а запрошено 10; 
        /// будет возвращено 5 элементов, но <see cref="CountToTake"/> будет содержать число 10.
        /// </summary>
        public int? CountToTake { get; set; }

        /// <summary>
        /// Количество пропускаемых элементов
        /// </summary>
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Признак вычислять ли общее количество элементов без учета <see cref="CountToSkip"/> и <see cref="CountToTake"/>.
        /// </summary>
        public bool? CalcTotalCount { get; set; }

        /// <summary>
        /// Контекст клиента.
        /// </summary>
        public Dictionary<string, string> ClientContext { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public WishListSortTypes SortType
        {
            get;
            set;
        }
        
        /// <summary>
        /// Направление сортировки
        /// </summary>
        public SortDirections SortDirect
        {
            get;
            set;
        }
    }
}