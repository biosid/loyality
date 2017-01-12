using System;

namespace RapidSoft.Loaylty.ProductCatalog.API
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using InputParameters;

    using Monitoring;

    using OutputResults;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [ServiceContract]
    public interface ICatalogAdminService : ISupportService
    {
        #region Категории

        /// <summary>
        /// Получение категорий
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns>Найденные категории</returns>
        [OperationContract]
        GetSubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters = null);

        /// <summary>
        /// Создание новой категории.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        CreateCategoryResult CreateCategory(CreateCategoryParameters parameters);

        /// <summary>
        /// Обновление категории.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        UpdateCategoryResult UpdateCategory(UpdateCategoryParameters parameters);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <returns>Результат операции</returns>
        [OperationContract]
        ResultBase DeleteCategory(string userId, int categoryId);

        /// <summary>
        /// Смена статуса категорий
        /// </summary>
        /// <param name="userId">Идентификатор клиента</param>
        /// <param name="categoryIds">Идентификаторы категорий</param>
        /// <param name="status">Устанавливаемый статус</param>
        /// <returns>Количество обновлённых категорий</returns>
        [OperationContract]
        ResultBase ChangeCategoriesStatus(string userId, int[] categoryIds, ProductCategoryStatuses status);

        /// <summary>
        /// Смена позиции категории
        /// </summary>
        /// <param name="parameters">
        /// Параметры перемещения.
        /// </param>
        /// <returns>
        /// Результат перемещения
        /// </returns>
        [OperationContract]
        ResultBase MoveCategory(MoveCategoryParameters parameters);

        #endregion

        #region Привзяка категорий на категории партнера

        /// <summary>
        /// Получение привязок рубрикатора к рубрикам партнёра 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <param name="categoryIds">
        /// The category Ids.
        /// </param>
        /// <returns>
        /// Привязки
        /// </returns>
        [OperationContract]
        GetPartnerProductCategoryLinksResult GetPartnerProductCategoryLinks(string userId, int partnerId, int[] categoryIds);

        /// <summary>
        /// Создание привязки категории рубрикатора к категории партнёра
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        CreatePartnerProductCategoryLinkResult SetPartnerProductCategoryLink(CreatePartnerProductCateroryLinkParameters parameters);

        /// <summary>
        /// Изменение разрешения на доступность категории для партнера.
        /// </summary>
        /// <param name="parameters">
        /// <see cref="SetCategoriesPermissionsParameters"/>.
        /// </param>
        /// <returns>
        /// The <see cref="ResultBase"/>.
        /// </returns>
        [OperationContract]
        ResultBase SetCategoriesPermissions(SetCategoriesPermissionsParameters parameters);

        /// <summary>
        /// Возвращает список идентификаторов разрешенных категории для партнера.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <returns>
        /// The <see cref="ResultBase"/>.
        /// </returns>
        [OperationContract]
        GetCategoriesPermissionsResult GetCategoriesPermissions(string userId, int partnerId);

        #endregion

        #region Партнер
        
        /// <summary>
        /// Создание нового партнера.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        CreatePartnerResult CreatePartner(CreatePartnerParameters parameters);

        /// <summary>
        /// Обновление партнера.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        UpdatePartnerResult UpdatePartner(UpdatePartnerParameters parameters);

        /// <summary>
        /// Поиск партнера.
        /// </summary>
        /// <param name="ids">Идентификаторы партнеров.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        GetPartnerResult GetPartners(int[] ids, string userId);

        [OperationContract]
        GetPartnersInfoResult GetPartnersInfo(int[] ids, string userId);

        /// <summary>
        /// Поиск партнера.
        /// </summary>
        /// <param name="id">Идентификатор партнера.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        GetPartnerByIdResult GetPartnerById(int id, string userId);

        [OperationContract]
        GetPartnerInfoByIdResult GetPartnerInfoById(int id, string userId);

        /// <summary>
        /// Сохранение настроек партнера.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        [OperationContract]
        ResultBase SetPartnerSettings(string userId, int partnerId, Dictionary<string, string> settings);

        /// <summary>
        /// Удаление настроек партнера.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <param name="keys">
        /// The keys.
        /// </param>
        [OperationContract]
        ResultBase DeletePartnerSettings(string userId, int partnerId, string[] keys);

        /// <summary>
        /// Получение настроек всех или заданого партнера
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="partnerId">Идентификатор партнера.</param>
        [OperationContract]
        PartnersSettignsResult GetPartnersSettings(string userId, int? partnerId);

        #endregion

        #region Импорт тарифов доставки

        /// <summary>
        /// Импорт списка тарифов доставки
        /// </summary>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <param name="fileUrl">
        /// The file Url.
        /// </param>
        /// <param name="userId">Идентификатор пользователя.</param>
        [OperationContract]
        ImportDeliveryRatesFromHttpResult ImportDeliveryRatesFromHttp(int partnerId, string fileUrl, string userId);

        /// <summary>
        /// Получение списка задач импорта
        /// </summary>
        [OperationContract]
        GetDeliveryRateImportTasksHistoryResult GetDeliveryRateImportTasksHistory(GetImportTasksHistoryParameters parameters);

        /// <summary>
        /// Операция предназначения для получения списка привязок с фильтрацией по партнеру
        /// </summary>
        /// <param name="parameters">Параметры отбора привязок</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат операции</returns>
        [OperationContract]
        DeliveryLocationsResult GetDeliveryLocations(GetDeliveryLocationsParameters parameters, string userId);

        /// <summary>
        /// Операция предназначения для сохранения кода КЛАДР в привязке
        /// </summary>
        /// <param name="locationId">Идентификатор привязки</param>
        /// <param name="kladr">Код КЛАДР</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат операции</returns>
        [OperationContract]
        ResultBase SetDeliveryLocationKladr(int locationId, string kladr, string userId);

        /// <summary>
        /// Операция предназначения для сброса кода КЛАРД в привязке.
        /// </summary>
        /// <param name="locationId">Идентификатор привязки</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат операции</returns>
        [OperationContract]
        ResultBase ResetDeliveryLocation(int locationId, string userId);

        /// <summary>
        /// Операция предназначена для получения истории привязки.
        /// </summary>
        /// <param name="parameters">Параметры отбора истории привязок</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат операции</returns>
        [OperationContract]
        DeliveryLocationHistoryResult GetDeliveryLocationHistory(GetDeliveryLocationHistoryParameters parameters, string userId);

        #endregion

        #region Импорт товаров

        /// <summary>
        /// Регистрация задачи импорта Yml-файла продукта. Файл должен храниться на web-сервере и доступен по протоколу HTTP
        /// </summary>
        [OperationContract]
        ImportProductsFromYmlResult ImportProductsFromYmlHttp(ImportProductsFromYmlHttpParameters parameters);

        /// <summary>
        /// Получение списка задач импорта
        /// </summary>
        [OperationContract]
        GetProductCatalogImportTasksHistoryResult GetProductCatalogImportTasksHistory(
            GetImportTasksHistoryParameters parameters);

        #endregion

        #region Товары

        /// <summary>
        /// Поиск всех товаров
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns>Найденные товары</returns>
        [OperationContract]
        SearchProductsResult SearchProducts(AdminSearchProductsParameters parameters);

        /// <summary>
        /// Получение товара по идентификатору
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns></returns>
        [OperationContract]
        AdminGetProductResult GetProductById(ArmGetProductByIdParameters parameters);

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [OperationContract]
        CreateProductResult CreateProduct(CreateProductParameters product);

        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ResultBase UpdateProduct(UpdateProductParameters parameters);

        /// <summary>
        /// Удаление товара
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        ResultBase DeleteProducts(DeleteProductParameters parameters);

        /// <summary>
        /// Перемещение продуктов
        /// </summary>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        [OperationContract]
        ResultBase MoveProducts(MoveProductsParameters parameters);

        /// <summary>
        /// Массовое инициализация целевых аудиторий для продуктов.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        ResultBase SetProductsTargetAudiences(SetProductsTargetAudiencesParameters parameters);

        /// <summary>
        /// Изменение статуса товаров
        /// </summary>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        [OperationContract]
        ResultBase ChangeProductsStatus(ChangeStatusParameters parameters);

        /// <summary>
        /// Изменение статуса товарова по партнеру и списку артикулов его товаров
        /// </summary>
        [OperationContract]
        ResultBase ChangeProductsStatusByPartner(ChangeStatusByPartnerParameters parameters);

        /// <summary>
        /// Изменение статуса модерации товаров
        /// </summary>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        [OperationContract]
        ResultBase ChangeProductsModerationStatus(ChangeModerationStatusParameters parameters);

        /// <summary>
        /// Изменение у товаров признака "рекомендованный"
        /// </summary>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        [OperationContract]
        ResultBase RecommendProducts(RecommendParameters parameters);

        [OperationContract]
        ResultBase DeleteCache(int seconds, string userId);

        [OperationContract]
        ResultBase SaveProductViewsForDay(DateTime date, KeyValuePair<string, int>[] views, string userId);
        
        #endregion

        #region Заказы

        /// <summary>
        /// Поиск заказов
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        SearchOrdersResult SearchOrders(SearchOrdersParameters parameters);

        /// <summary>
        /// Получение заказа по идентификатору
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperationContract]
        GetOrderResult GetOrderById(string userId, int orderId);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ordersStatuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersStatuses(string userId, OrdersStatus[] ordersStatuses);

        [OperationContract]
        ResultBase ChangeOrdersStatusDescription(string userId, int orderId, string orderStatusDescription);

        /// <summary>
        /// Обновление статусов оплаты заказов
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(string userId, OrdersPaymentStatus[] statuses);

        /// <summary>
        /// Обновление статусов доставки заказов
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(string userId, OrdersDeliveryStatus[] statuses);

        /// <summary>
        /// Получение истории статусов заказа
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        GetOrderStatusesHistoryResult GetOrderStatusesHistory(GetOrderStatusesHistoryParameters parameters);

        /// <summary>
        /// Подтверждение заказа партнёром
        /// </summary>
        /// <param name="partnerId">Идентификатор партнера.</param>
        /// <param name="partnerOrderCommitment">
        /// Набор <see cref="PartnerOrderCommitment"/>.
        /// </param>
        /// <returns>
        /// <see cref="PartnerCommitOrdersResult"/>
        /// </returns>
        [OperationContract]
        PartnerCommitOrdersResult PartnerCommitOrder(string userId, int partnerId, PartnerOrderCommitment[] partnerOrderCommitment);

        /// <summary>
        /// Обновление инструкции по получению заказа
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [OperationContract]
        ResultBase ChangeOrderDeliveryInstructions(string userId, int orderId, string instructions);

        #endregion
    }
}