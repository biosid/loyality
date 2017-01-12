namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Интерфейс репозитория для работы с задачами импорта <see cref="ProductImportTask"/> и <see cref="DeliveryRateImportTask"/>.
    /// </summary>
    public interface IImportTaskRepository
    {
        /// <summary>
        /// Сохранение в БД задачи импорта каталога товаров.
        /// </summary>
        /// <param name="task">
        /// Сохраняемая задача.
        /// </param>
        /// <returns>
        /// Сохраненая задача.
        /// </returns>
        ProductImportTask SaveProductImportTask(ProductImportTask task);

        /// <summary>
        /// Получение задачи импорта каталога товаров по идентификатору.
        /// </summary>
        /// <param name="id">
        /// Идентификатор задачи.
        /// </param>
        /// <returns>
        /// Найденная задача.
        /// </returns>
        ProductImportTask GetProductImportTask(int id);

        /// <summary>
        /// Получение ограниченного по кол-ву набора задач импорта каталога товаров.
        /// </summary>
        /// <param name="partnerId">
        /// Идентификатор партнера.
        /// </param>
        /// <param name="skipCount">
        /// Кол-во пропускаемых задач.
        /// </param>
        /// <param name="takeCount">
        /// Кол-во возвращаемых задач.
        /// </param>
        /// <param name="calcTotalCount">
        /// Признак указывающий на необходимость вычислить кол-во задач без учета <paramref name="skipCount"/> и <paramref name="takeCount"/>.
        /// </param>
        /// <returns>
        /// Набор найденных задач.
        /// </returns>
        Page<ProductImportTask> GetPageProductImportTask(int? partnerId, int? skipCount, int? takeCount, bool? calcTotalCount);

        /// <summary>
        /// Получение ограниченного по кол-ву набора задач импорта тарифов доставки.
        /// </summary>
        /// <param name="partnerId">
        /// Идентификатор партнера.
        /// </param>
        /// <param name="skipCount">
        /// Кол-во пропускаемых задач.
        /// </param>
        /// <param name="takeCount">
        /// Кол-во возвращаемых задач.
        /// </param>
        /// <param name="calcTotalCount">
        /// Признак указывающий на необходимость вычислить кол-во задач без учета <paramref name="skipCount"/> и <paramref name="takeCount"/>.
        /// </param>
        /// <returns>
        /// Набор найденных задач.
        /// </returns>
        Page<DeliveryRateImportTask> GetPageDeliveryRateImportTask(int? partnerId, int? skipCount, int? takeCount, bool? calcTotalCount);
    }
}
