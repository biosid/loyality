namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Страница сущностей, используется вместо <see cref="List"/> или любой другой коллекции, так как последнии не содержат информации о том что в коллекции не все данные
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class Page<TEntity> : List<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Page{TEntity}"/> class.
        /// </summary>
        /// <param name="collection">
        /// Коллекция элементов.
        /// </param>
        /// <param name="skipCount">
        /// Кол-во пропущенных элементов при формировании страницы.
        /// </param>
        /// <param name="takeCount">
        /// Кол-во извлеченых элементов при формировании страницы.
        /// </param>
        /// <param name="totalCount">
        /// Кол-во элементов без учета количеств пропущенных и извлеченных.
        /// </param>
        public Page(IEnumerable<TEntity> collection, int? skipCount = null, int? takeCount = null, int? totalCount = null)
            : base(collection)
        {
            this.SkipCount = skipCount ?? 0;
            this.TakeCount = takeCount;
            this.TotalCount = totalCount;
        }

        public Page(int? skipCount = null, int? takeCount = null)
        {
            this.SkipCount = skipCount ?? 0;
            this.TakeCount = takeCount;
            this.TotalCount = 0;
        }

        /// <summary>
        /// Кол-во пропущенных элементов при формировании страницы.
        /// </summary>
        public int SkipCount { get; private set; }

        /// <summary>
        /// Кол-во извлеченых элементов при формировании страницы.
        /// </summary>
        public int? TakeCount { get; private set; }

        /// <summary>
        /// Кол-во элементов без учета количеств пропущенных и извлеченных.
        /// </summary>
        public int? TotalCount { get; private set; }
    }
}