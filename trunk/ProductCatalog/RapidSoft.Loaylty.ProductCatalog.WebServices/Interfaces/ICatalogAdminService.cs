namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [ServiceContract]
    public interface ICatalogAdminService : ISupportService
    {
        #region Категории

        /// <summary>
        /// Получение категорий
        /// </summary>
        [OperationContract]
        SubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters);

        /// <summary>
        /// Создание новой категории.
        /// </summary>
        [OperationContract]
        ValueResult<ProductCategory> CreateCategory(CreateCategoryParameters parameters);

        /// <summary>
        /// Обновление категории.
        /// </summary>
        [OperationContract]
        ValueResult<ProductCategory> UpdateCategory(UpdateCategoryParameters parameters);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        [OperationContract]
        ResultBase DeleteCategory(DeleteCategoryParameters parameters);

        /// <summary>
        /// Смена статуса категорий
        /// </summary>
        [OperationContract]
        ResultBase ChangeCategoriesStatus(ChangeCategoriesStatusParameters parameters);

        /// <summary>
        /// Смена позиции категории
        /// </summary>
        [OperationContract]
        ResultBase MoveCategory(MoveCategoryParameters parameters);

        #endregion

        #region Привзяка категорий на категории партнера

        /// <summary>
        /// Получение привязок рубрикатора к рубрикам партнёра 
        /// </summary>
        [OperationContract]
        ArrayResult<PartnerCategoryLink> GetPartnerProductCategoryLinks(GetPartnerProductCategoryLinksParameters parameters);

        /// <summary>
        /// Создание привязки категории рубрикатора к категории партнёра
        /// </summary>
        [OperationContract]
        ValueResult<PartnerCategoryLink> SetPartnerProductCategoryLink(SetPartnerProductCategoryLinkParameters parameters);

        /// <summary>
        /// Изменение разрешения на доступность категории для партнера.
        /// </summary>
        [OperationContract]
        ResultBase SetCategoriesPermissions(SetCategoriesPermissionsParameters parameters);

        /// <summary>
        /// Возвращает список идентификаторов разрешенных категории для партнера.
        /// </summary>
        [OperationContract]
        ArrayResult<int> GetCategoriesPermissions(GetCategoriesPermissionsParameters parameters);

        #endregion

        #region Партнер
        
        /// <summary>
        /// Создание нового партнера.
        /// </summary>
        [OperationContract]
        ValueResult<Partner> CreatePartner(CreatePartnerParameters parameters);

        /// <summary>
        /// Обновление партнера.
        /// </summary>
        [OperationContract]
        ValueResult<Partner> UpdatePartner(UpdatePartnerParameters parameters);

        /// <summary>
        /// Поиск партнера.
        /// </summary>
        [OperationContract]
        ArrayResult<Partner> GetPartners(GetPartnersParameters parameters);

        [OperationContract]
        ArrayResult<PartnerInfo> GetPartnersInfo(GetPartnersParameters parameters);

        /// <summary>
        /// Поиск партнера.
        /// </summary>
        [OperationContract]
        ValueResult<Partner> GetPartnerById(GetPartnerByIdParameters parameters);

        [OperationContract]
        ValueResult<PartnerInfo> GetPartnerInfoById(GetPartnerByIdParameters parameters);

        /// <summary>
        /// Сохранение настроек партнера.
        /// </summary>
        [OperationContract]
        ResultBase SetPartnerSettings(SetPartnerSettingsParameters parameters);

        /// <summary>
        /// Удаление настроек партнера.
        /// </summary>
        [OperationContract]
        ResultBase DeletePartnerSettings(DeletePartnerSettingsParameters parameters);

        /// <summary>
        /// Получение настроек всех или заданого партнера
        /// </summary>
        [OperationContract]
        ArrayResult<PartnerSetting> GetPartnersSettings(GetPartnersSettingsParameters parameters);

        #endregion

        #region Импорт тарифов доставки

        /// <summary>
        /// Импорт списка тарифов доставки
        /// </summary>
        [OperationContract]
        ValueResult<string> ImportDeliveryRatesFromHttp(ImportDeliveryRatesFromHttpParameters parameters);

        /// <summary>
        /// Получение списка задач импорта
        /// </summary>
        [OperationContract]
        PagedResult<DeliveryRateImportTask> GetDeliveryRateImportTasksHistory(GetDeliveryRateImportTasksHistoryParameters parameters);

        /// <summary>
        /// Операция предназначения для получения списка привязок с фильтрацией по партнеру
        /// </summary>
        [OperationContract]
        PagedResult<DeliveryLocation> GetDeliveryLocations(GetDeliveryLocationsParameters parameters);

        /// <summary>
        /// Операция предназначения для сохранения кода КЛАДР в привязке
        /// </summary>
        [OperationContract]
        ResultBase SetDeliveryLocationKladr(SetDeliveryLocationKladrParameters parameters);

        /// <summary>
        /// Операция предназначения для сброса кода КЛАРД в привязке.
        /// </summary>
        [OperationContract]
        ResultBase ResetDeliveryLocation(ResetDeliveryLocationParameters parameters);

        /// <summary>
        /// Операция предназначена для получения истории привязки.
        /// </summary>
        [OperationContract]
        PagedResult<DeliveryLocationHistory> GetDeliveryLocationHistory(GetDeliveryLocationHistoryParameters parameters);

        #endregion

        #region Импорт товаров

        /// <summary>
        /// Регистрация задачи импорта Yml-файла продукта. Файл должен храниться на web-сервере и доступен по протоколу HTTP
        /// </summary>
        [OperationContract]
        ValueResult<int?> ImportProductsFromYmlHttp(ImportProductsFromYmlHttpParameters parameters);

        /// <summary>
        /// Получение списка задач импорта
        /// </summary>
        [OperationContract]
        PagedResult<ProductImportTask> GetProductCatalogImportTasksHistory(GetProductCatalogImportTasksHistoryParameters parameters);

        #endregion

        #region Товары

        /// <summary>
        /// Поиск всех товаров
        /// </summary>
        [OperationContract]
        AdminProductsResult SearchProducts(SearchProductsParameters parameters);

        /// <summary>
        /// Получение товара по идентификатору
        /// </summary>
        [OperationContract]
        ValueResult<AdminProduct> GetProductById(GetProductByIdAdminParameters parameters);

        /// <summary>
        /// Добавление продукта
        /// </summary>
        [OperationContract]
        ValueResult<AdminProduct> CreateProduct(CreateProductParameters product);

        /// <summary>
        /// Обновление продукта
        /// </summary>
        [OperationContract]
        ResultBase UpdateProduct(UpdateProductParameters parameters);

        /// <summary>
        /// Удаление товара
        /// </summary>
        [OperationContract]
        ResultBase DeleteProducts(DeleteProductsParameters parameters);

        /// <summary>
        /// Перемещение продуктов
        /// </summary>
        [OperationContract]
        ResultBase MoveProducts(MoveProductsParameters parameters);

        /// <summary>
        /// Массовое инициализация целевых аудиторий для продуктов.
        /// </summary>
        [OperationContract]
        ResultBase SetProductsTargetAudiences(SetProductsTargetAudiencesParameters parameters);

        /// <summary>
        /// Изменение статуса товаров
        /// </summary>
        [OperationContract]
        ResultBase ChangeProductsStatus(ChangeProductsStatusParameters parameters);

        /// <summary>
        /// Изменение статуса модерации товаров
        /// </summary>
        [OperationContract]
        ResultBase ChangeProductsModerationStatus(ChangeProductsModerationStatusParameters parameters);

        /// <summary>
        /// Изменение у товаров признака "рекомендованный"
        /// </summary>
        [OperationContract]
        ResultBase RecommendProducts(RecommendProductsParameters parameters);

        [OperationContract]
        ResultBase DeleteCache(DeleteCacheParameters parameters);

        #endregion

        #region Заказы

        /// <summary>
        /// Поиск заказов
        /// </summary>
        [OperationContract]
        PagedResult<Order> SearchOrders(SearchOrdersParameters parameters);

        /// <summary>
        /// Получение заказа по идентификатору
        /// </summary>
        [OperationContract]
        OrderResult GetOrderById(GetOrderByIdParameters parameters);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeOrdersStatuses(ChangeOrdersStatusesParameters parameters);

        [OperationContract]
        ResultBase ChangeOrdersStatusDescription(ChangeOrdersStatusDescriptionParameters parameters);

        /// <summary>
        /// Обновление статусов оплаты заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeProductsPaymentStatuses(ChangePaymentStatusesParameters parameters);

        /// <summary>
        /// Обновление статусов доставки заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeDeliveryPaymentStatuses(ChangePaymentStatusesParameters parameters);

        /// <summary>
        /// Получение истории статусов заказа
        /// </summary>
        [OperationContract]
        PagedResult<OrderHistory> GetOrderStatusesHistory(GetOrderStatusesHistoryParameters parameters);

        /// <summary>
        /// Подтверждение заказа партнёром
        /// </summary>
        [OperationContract]
        ArrayResult<OrderIdentity> PartnerCommitOrder(PartnerCommitOrderParameters parameters);

        #endregion
    }
}