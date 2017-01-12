using System.Collections.Generic;

namespace RapidSoft.Loaylty.ProductCatalog.API
{
    using System;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    /// <summary>
    /// Интерфейс сервиса "Корзина клиента"
    /// </summary>
    [ServiceContract]
    public interface IBasketService : ISupportService
    {
        /// <summary>
        /// Добавление товара в корзину клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="quantity">Количество товаров</param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        BasketManageResult Add(string clientId, string productId, int quantity = 1, Dictionary<string, string> clientContext = null);

        /// <summary>
        /// Установка количества товаров в корзине клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="quantity">Количество товаров</param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        BasketManageResult SetQuantity(string clientId, string productId, int quantity, Dictionary<string, string> clientContext = null);

        /// <summary>
        /// Удаление товара из Корзины клиента
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        /// Внутренний идентификатор товара.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        BasketManageResult Remove(string clientId, string productId);

        /// <summary>
        /// Получение элемента Корзины
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        /// Внутренний идентификатор товара.
        /// </param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        GetBasketItemResult GetBasketItem(string clientId, string productId, Dictionary<string, string> clientContext);

        /// <summary>
        /// Получение элементов Корзины
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productIds">
        /// Список внутренних идентификаторов товаров.
        /// </param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        GetBasketItemsResult GetBasketItems(string clientId, string[] productIds, Dictionary<string, string> clientContext);

        /// <summary>
        /// Получение элемента Корзины
        /// </summary>
        /// <param name="basketItemId"></param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        GetBasketItemResult GetBasketItem(Guid basketItemId, Dictionary<string, string> clientContext);

        /// <summary>
        /// Получение элементов Корзины
        /// </summary>
        /// <param name="basketItemIds"></param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        GetBasketItemsResult GetBasketItems(Guid[] basketItemIds, Dictionary<string, string> clientContext);

        /// <summary>
        /// Получение перечня товаров из Корзины клиента
        /// </summary>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        BasketResult GetBasket(GetBasketParameters parameters);
    }
}
