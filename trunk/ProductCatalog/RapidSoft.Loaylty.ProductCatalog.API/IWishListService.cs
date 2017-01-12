namespace RapidSoft.Loaylty.ProductCatalog.API
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    /// <summary>
    /// Интерфейс управления WishList клиента.
    /// </summary>
    [ServiceContract]
    public interface IWishListService : ISupportService
    {
        /// <summary>
        /// Добавление товара в WishList.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор пользователя/клиента.
        /// </param>
        /// <param name="productId">
        ///     Идентификатор товара.
        /// </param>
        /// <param name="quantity">
        ///     Количество товара в штуках.
        /// </param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат добавление товара в WishList.
        /// </returns>
        [OperationContract]
        WishListResult Add(string clientId, string productId, int quantity = 1, Dictionary<string, string> clientContext = null);

        /// <summary>
        /// Изменение количества товаров в WishList клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="quantity">
        ///     Количество товаров
        /// </param>
        /// <param name="clientContext"></param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        WishListResult SetQuantity(string clientId, string productId, int quantity, Dictionary<string, string> clientContext = null);

        /// <summary>
        /// Удаление товара из WishList клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        WishListResult Remove(string clientId, string productId);

        /// <summary>
        /// Получение элемента WishList клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="clientContext">
        ///     Набор пар ключ-значение, содержащий информацию о клиенте.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        [OperationContract]
        GetWishListItemResult GetWishListItem(string clientId, string productId, Dictionary<string, string> clientContext);

        /// <summary>
        /// Получение перечня товаров из WishList клиента.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Отобранные продукты.
        /// </returns>
        [OperationContract]
        GetWishListResult GetWishList(GetWishListParameters parameters);

        /// <summary>
        /// make wishlist notifications
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ResultBase MakeWishListNotifications();

        [OperationContract]
        GetWishListNotificationsResult GetWishListNotifications(GetWishListNotificationsParameters parameters);
    }
}