namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    /// <summary>
    /// Репозиторий для работы с элементами корзины.
    /// </summary>
    public class BasketItemRepository : IBasketItemRepository
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketItemRepository"/> class.
        /// </summary>
        public BasketItemRepository()
        {
            this.connectionString = DataSourceConfig.ConnectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketItemRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения.
        /// </param>
        public BasketItemRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

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
        public BasketItem Get(string clientId, string productId)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                return this.InternalGet(clientId, productId, ctx);
            }
        }

        /// <summary>
        /// Возвращает элементы корзины для заданого пользователя и списка продуктов.
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productIds">
        /// Список внутреннийх идентификаторов товаров.
        /// </param>
        /// <returns>
        /// Найденные элементы корзины.
        /// </returns>
        public BasketItem[] Get(string clientId, string[] productIds)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var basketItems = ctx.BasketItems.Where(x => x.ClientId == clientId && productIds.Contains(x.ProductId)).ToArray();
                return productIds.Select(p => basketItems.FirstOrDefault(i => i.ProductId == p)).ToArray();
            }
        }

        public BasketItem Get(Guid basketItemId)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                return ctx.BasketItems.SingleOrDefault(x => x.Id == basketItemId);
            }
        }

        public BasketItem[] Get(Guid[] basketItemIds)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var basketItems = ctx.BasketItems.Where(x => basketItemIds.Contains(x.Id)).ToArray();
                return basketItemIds.Select(b => basketItems.FirstOrDefault(i => i.Id == b)).ToArray();
            }
        }

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
        public IList<BasketItem> GetByClientId(string clientId, int skip = 0, int? take = null)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var query = ctx.BasketItems.Where(x => x.ClientId == clientId).OrderByDescending(x => x.CreatedDate).Skip(skip);

                if (take != null)
                {
                    var takeCount = take.Value;
                    query = query.Take(takeCount);
                }

                return query.ToList();
            }
        }

        public int GetCountByClientId(string clientId)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                return ctx.BasketItems.Count(x => x.ClientId == clientId);
            }
        }

        public BasketItem Add(string clientId, string productId, int? quantity, FixedPrice fixedPrice = null, Guid? newId = null, int? basketItemGroupId = null)
        {
            if (quantity.HasValue)
            {
                this.CheckMaxQuantity(quantity.Value);
            }

            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var item = new BasketItem
                {
                    Id = newId ?? Guid.NewGuid(),
                    ClientId = clientId, 
                    ProductId = productId, 
                    ProductsQuantity = quantity ?? 1, 
                    CreatedDate = DateTime.Now,
                    FixedPrice = fixedPrice == null ? null : XmlSerializer.Serialize(fixedPrice),
                    BasketItemGroupId = basketItemGroupId
                };

                ctx.BasketItems.Add(item);

                ctx.SaveChanges();

                return item;
            }
        }

        public BasketItem Update(string clientId, string productId, int quantity, FixedPrice fixedPrice)
        {
            this.CheckMaxQuantity(quantity);

            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var existed = this.InternalGet(clientId, productId, ctx);

                if (existed != null)
                {
                    existed.ProductsQuantity = quantity;
                    existed.FixedPrice = fixedPrice == null ? null : XmlSerializer.Serialize(fixedPrice);
                    ctx.SaveChanges();
                }

                return existed;
            }
        }

        public void Remove(string clientId, string productId)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var existed = this.InternalGet(clientId, productId, ctx);

                if (existed != null)
                {
                    ctx.BasketItems.Remove(existed);

                    ctx.SaveChanges();
                }
            }
        }

        private BasketItem InternalGet(string clientId, string productId, LoyaltyDBEntities ctx)
        {
            return ctx.BasketItems.SingleOrDefault(x => x.ClientId == clientId && x.ProductId == productId);
        }

        private void CheckMaxQuantity(int quantity)
        {
            if (quantity > ApiSettings.MaxBasketItemProductsQuantity)
            {
                var mess = string.Format(
                    "Превышено максимальное кол-во желаемых единиц продукта: max = {0}, желаемое = {1}",
                    ApiSettings.MaxBasketItemProductsQuantity,
                    quantity);
                throw new OperationException(ResultCodes.QUANTITY_OVERFLOW, mess);
            }
        }
    }
}