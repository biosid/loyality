namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;
    using System.Collections.Generic;

    using PromoAction.WsClients.MechanicsService;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    public interface IProductCategoriesDataSource
    {
        /// <summary>
        /// Получение окна подкатегорий
        /// </summary>
        /// <param name="priceSql">
        /// Sql для расчёта цены товара
        /// </param>
        /// <param name="locationCode">
        /// КЛАДР код местонахождения пользователя
        /// </param>
        /// <param name="categoriesStatus">
        /// Категории с каким статусом необходимо получить
        /// </param>
        /// <param name="parentId">
        /// Идентификатор родительской категории
        /// </param>
        /// <param name="nestingLevel">
        /// Требуемый уровень вложенности
        /// </param>
        /// <param name="countToTake">
        /// Сколько категорий взять
        /// </param>
        /// <param name="countToSkip">
        /// Сколько категорий необходимо пропустить
        /// </param>
        /// <param name="calcTotalCount">
        /// Необходимо ли считать обще количество подкатегорий
        /// </param>
        /// <param name="audienceIds">
        /// Перечень целевых аудиторий в которые входит пользователь. Разделитель запятая ","
        /// </param>
        /// <param name="includeParent">
        /// Включать в выдачу каталог родитель
        /// </param>
        /// <param name="type">
        /// Фильтр по типу категории
        /// </param>
        /// <param name="categoriesIds">
        /// The categories Ids.
        /// </param>
        /// <returns>
        /// Категории
        /// </returns>
        ProductCategorySearchResult GetPublicCategories(GenerateResult priceSql, string locationCode = null, ProductCategoryStatuses? categoriesStatus = null, int? parentId = null, int? nestingLevel = null, int? countToTake = null, int? countToSkip = null, bool? calcTotalCount = null, string audienceIds = null, bool includeParent = false, ProductCategoryTypes? type = null, int[] categoriesIds = null);

        /// <summary>
        /// Получение списка категорий без доп ограничений публичного доступа: 
        /// 1. игнорируются тарифы доставки товаров, 
        /// 2. игнорируется таргетирование продуктов,
        /// 3. игнорируются статусы товаров.
        /// </summary>
        /// <param name="categoriesStatus">Категории с каким статусом необходимо получить</param>
        /// <param name="parentId">Идентификатор родительской категории</param>
        /// <param name="nestingLevel">Требуемый уровень вложенности</param>
        /// <param name="countToTake">Сколько категорий взять</param>
        /// <param name="countToSkip">Сколько категорий необходимо пропустить</param>
        /// <param name="calcTotalCount">Необходимо ли считать обще количество подкатегорий</param>
        /// <param name="includeParent">Включать в выдачу каталог родитель</param>
        /// <param name="type">Фильтр по типу категории</param>
        /// <returns>Категории</returns>
        ProductCategorySearchResult AdminGetCategories(
            ProductCategoryStatuses? categoriesStatus = null,
            int? parentId = null,
            int? nestingLevel = null,
            int? countToTake = null,
            int? countToSkip = null,
            bool? calcTotalCount = null,
            bool includeParent = false,
            ProductCategoryTypes? type = null);

        /// <summary>
        /// Получение перечня родительских категорий
        /// </summary>
        /// <param name="categoryId">Идентификатор категории, для которой необходимо получить все её ролительские категории</param>
        /// <returns>Категории отсортированные в порядке возрастания NamePath</returns>
        ProductCategory[] GetParentCategoriesPath(int categoryId);

        /// <summary>
        /// Создание категории
        /// </summary>
        /// <param name="parameters">Параметры новой категории</param>
        /// <returns>Созданную категорию</returns>
        ProductCategory CreateProductCategory(CreateCategoryParameters parameters);

        /// <summary>
        /// Обновление категории
        /// </summary>
        /// <param name="parameters">Параметры обновляемой категории</param>
        /// <returns>Обновлённую категорию</returns>
        ProductCategory UpdateCategory(UpdateCategoryParameters parameters);

        ProductCategory GetProductCategoryById(int id);

        ProductCategory[] GetProductCategoriesByIds(IEnumerable<int> existedCategoryIds);
    }
}