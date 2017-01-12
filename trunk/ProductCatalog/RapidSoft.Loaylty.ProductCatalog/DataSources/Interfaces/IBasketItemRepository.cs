namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Интефейс репозитория корзины клиента.
    /// </summary>
    public interface IBasketItemRepository
    {
        /// <summary>
        /// Возвращает элемент корзины для заданого пользователя и продукта.
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        /// Внутренний идентификатор товара.
        /// </param>
        /// <returns>
        /// Найденный элемент корзины или <c>null</c>.
        /// </returns>
        BasketItem Get(string clientId, string productId);

        BasketItem[] Get(string clientId, string[] productIds);

        BasketItem Get(Guid basketItemId);

        BasketItem[] Get(Guid[] basketItemId);

        /// <summary>
        /// Получение набора элементов корзины для заданного клиента.
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="skip">
        /// Количество элементов корзины которые необходимо пропустить. По умолчанию 0.
        /// </param>
        /// <param name="take">
        /// Количество элементов корзины.
        /// </param>
        /// <returns>
        /// Набор элементов корзины.
        /// </returns>
        IList<BasketItem> GetByClientId(string clientId, int skip = 0, int? take = null);

        int GetCountByClientId(string clientId);

        BasketItem Add(string clientId, string productId, int? quantity, FixedPrice fixedPrice = null, Guid? newId = null, int? basketItemGroupId = null);

        BasketItem Update(string clientId, string productId, int quantity, FixedPrice fixedPrice);

        void Remove(string clientId, string productId);
    }
}