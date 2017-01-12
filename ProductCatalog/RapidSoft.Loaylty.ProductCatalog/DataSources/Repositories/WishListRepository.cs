namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using API.Entities;
    using API.InputParameters;
    using API.OutputResults;
    using Extensions;

    using Interfaces;

    using Services;

    using Settings;

    using WishListItem = API.Entities.WishListItem;

    public class WishListRepository : IWishListRepository
    {
        private readonly string connectionString = DataSourceConfig.ConnectionString;

        #region IWishListRepository Members

        public int GetCountByClientId(string clientId)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var list = ctx.WishListItems.Where(i => i.ClientId == clientId)
                              .Join(
                                  ctx.ProductSortProjections,
                                  i => i.ProductId,
                                  p => p.ProductId,
                                  (item, product) => new
                                                     {
                                                         item,
                                                         product
                                                     });

                var retVal = list.Count();
                return retVal;
            }
        }

        public WishListItem CreateOrUpdate(string productId, string clientId, int quantity)
        {
            productId.ThrowIfNull("productId");
            clientId.ThrowIfNull("clientId");
            if (quantity < 1)
            {
                throw new ArgumentException("Кол-во элементов (quantity) не может быть меньше 1");
            }

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var existed = ctx.WishListItems.SingleOrDefault(x => x.ProductId == productId && x.ClientId == clientId);

                this.CheckMaxQuantity(quantity);

                if (existed == null)
                {
                    var date = DateTime.Now;

                    existed = new WishListItem
                    {
                        CreatedDate = date, 
                        ClientId = clientId, 
                        ProductId = productId, 
                        ProductsQuantity = quantity
                    };

                    ctx.Entry(existed).State = EntityState.Added;
                }
                else
                {
                    existed.ProductsQuantity = quantity;
                    ctx.Entry(existed).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                return existed;
            }
        }

        public WishListItem IncreaseQuantityOrCreate(string productId, string clientId, int quantity = 1)
        {
            productId.ThrowIfNull("productId");
            clientId.ThrowIfNull("clientId");

            if (quantity < 1)
            {
                throw new ArgumentException("Кол-во элементов (quantity) не может быть меньше 1");
            }

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var existed = ctx.WishListItems.FirstOrDefault(wli => wli.ClientId == clientId && wli.ProductId == productId);

                quantity = existed == null ? quantity : existed.ProductsQuantity + quantity;

                this.CheckMaxQuantity(quantity);

                if (existed == null)
                {
                    existed = new WishListItem
                    {
                        CreatedDate = DateTime.Now, 
                        ClientId = clientId, 
                        ProductId = productId, 
                        ProductsQuantity = quantity
                    };

                    ctx.Entry(existed).State = EntityState.Added;
                }
                else
                {
                    existed.ProductsQuantity = quantity;
                    existed.CreatedDate = DateTime.Now;
                    ctx.Entry(existed).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                return existed;
            }
        }

        /// <summary>
        /// Возвращает элемент WishList для заданого пользователя и продукта.
        /// </summary>
        /// <param name="productId">
        /// Внутренний идентификатор товара.
        /// </param>
        /// <param name="clientId">
        /// Идентификатор клиента/пользователя.
        /// </param>
        /// <returns>
        /// Найденный элемент WishList или <c>null</c>.
        /// </returns>
        public WishListItem Get(string productId, string clientId)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var retVal = ctx.WishListItems.SingleOrDefault(x => x.ProductId == productId && x.ClientId == clientId);
                return retVal;
            }
        }

        public bool Remove(string productId, string clientId)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var linkedNotification = ctx.WishListItemNotifications.FirstOrDefault(wlin => wlin.ClientId == clientId && wlin.ProductId == productId);

                if (linkedNotification != null)
                {
                    ctx.Entry(linkedNotification).State = EntityState.Deleted;

                    ctx.SaveChanges();
                }

                var existed = ctx.WishListItems.FirstOrDefault(wli => wli.ClientId == clientId && wli.ProductId == productId);

                if (existed != null)
                {
                    ctx.Entry(existed).State = EntityState.Deleted;

                    ctx.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public Page<Notification> GetItemsToNotify(string clientId, int? take = null, bool? calcTotal = null)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                return GetItemsToNotify(ctx, clientId, take, calcTotal);
            }
        }

        public Page<Notification> GetItemsToNotify(LoyaltyDBEntities context, string clientId, int? take = null, bool? calcTotal = null)
        {
            var skip = 0;
            var targetNotifications = context.WishListItemNotifications.Where(n => n.NotificationDate == null);

            if (!string.IsNullOrEmpty(clientId))
            {
                targetNotifications = targetNotifications.Where(x => x.ClientId == clientId);
            }

            var query = from n in targetNotifications
                        join p in context.ProductSortProjections on n.ProductId equals p.ProductId
                        join wi in context.WishListItems on new { n.ClientId, n.ProductId } equals new { wi.ClientId, wi.ProductId }
                        where n.NotificationDate == null
                        orderby new { n.ClientId, n.ProductId }
                        select new
                        {
                            Notification = n,
                            Result = new Notification
                            {
                                ClientId = n.ClientId,
                                ProductId = n.ProductId,
                                ProductName = p.Name,
                                ProductQuantity = n.ProductsQuantity,
                                FirstName = n.FirstName,
                                MiddleName = n.MiddleName,
                                ItemBonusCost = n.ItemBonusCost,
                                TotalBonusCost = n.TotalBonusCost
                            }
                        };

            int? totalCount = null;
            if (calcTotal.HasValue && calcTotal.Value)
            {
                totalCount = query.Count();
            }

            query = SkipTake(query, skip, take);

            var notificationsWithInfo = query.ToList();

             var now = DateTime.Now;
             notificationsWithInfo.ForEach(n => n.Notification.NotificationDate = now);

             context.SaveChanges();

            var notifications = notificationsWithInfo.Select(x => x.Result);

            var retVal = new Page<Notification>(notifications, skip, take, totalCount);
            return retVal;
        }

        public void CleanUpNonSentNotifications()
        {
            using (var context = new LoyaltyDBEntities(connectionString))
            {
                var notifs = from n in context.WishListItemNotifications
                             where n.NotificationDate == null
                             select n;
                foreach (var notification in notifs)
                {
                    context.WishListItemNotifications.Remove(notification);
                }

                context.SaveChanges();
            }
        }

        public int GetWishListCount(string clientId)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                return ctx.WishListItems.Count(wli => wli.ClientId == clientId);
            }
        }

        public IList<WishListItem> GetWishList(string clientId)
        {
            List<WishListItem> result;

            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                result = ctx.WishListItems.Where(wli => wli.ClientId == clientId).ToList();
            }

            return result;
        }

        public IList<WishListItem> GetWishList(string clientId, WishListSortTypes sortType, SortDirections sortDirect, int? skip = 0, int? take = null)
        {
            using (var ctx = new LoyaltyDBEntities(connectionString))
            {
                var list = ctx.WishListItems
                              .Where(i => i.ClientId == clientId)
                              .Join(
                              ctx.ProductSortProjections, 
                              i => i.ProductId, 
                              p => p.ProductId, 
                                  (item, product) => new
                                  {
                                      item, 
                                      product
                                  });

                IQueryable<WishListItem> orderedList;
                switch (sortType)
                {
                    case WishListSortTypes.ByCreateDate:
                    {
                        orderedList =
                            OrderByDirection(list, pair => pair.item.CreatedDate, sortDirect).Select(x => x.item);
                        break;
                    }

                    case WishListSortTypes.ByProductName:
                    {
                        orderedList = OrderByDirection(list, pair => pair.product.Name, sortDirect).Select(x => x.item);
                        break;
                    }

                    default:
                    {
                        var mess = string.Format("Тип сортировки {0} не поддерживается", sortType);
                        throw new NotSupportedException(mess);
                    }
                }

                var retVal = SkipTake(orderedList, skip, take).ToList();
                return retVal;
            }
        }

        public List<WishListItemNotification> GetWishListToNotify()
        {
            using (var context = new LoyaltyDBEntities(connectionString))
            {
                var now = DateTime.Now;
                var wishListToNotify = context.Database.SqlQuery<WishListItem>("[prod].[GetWishListToNotify]");
                var itemsNotifications = wishListToNotify.Select(resItems => new WishListItemNotification(resItems, now)).ToList();
                return itemsNotifications;
            }
        }

        public void AddNotifications(List<WishListItemNotification> notifications)
        {
            using (var context = new LoyaltyDBEntities(connectionString))
            {
                notifications.ForEach(x => context.WishListItemNotifications.Add(x));
                context.SaveChanges();
            }
        }

        #endregion

        private IQueryable<TSource> SkipTake<TSource>(IQueryable<TSource> list, int? skip = 0, int? take = null)
        {
            var retList = list;
            if (skip.HasValue && skip != 0)
            {
                retList = retList.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                retList = retList.Take(take.Value);
            }

            return retList;
        }

        private IOrderedQueryable<TSource> OrderByDirection<TSource, TKey>(IQueryable<TSource> list, Expression<Func<TSource, TKey>> keySelector, SortDirections sortDirect)
        {
            switch (sortDirect)
            {
                case SortDirections.Asc:
                {
                    return list.OrderBy(keySelector);
                }

                case SortDirections.Desc:
                {
                    return list.OrderByDescending(keySelector);
                }

                default:
                {
                    var mess = string.Format("Направление сортировки {0} не поддерживается", sortDirect);
                    throw new NotSupportedException(mess);
                }
            }
        }

        private void CheckMaxQuantity(int quantity)
        {
            if (quantity > ApiSettings.MaxWishListItemProductsQuantity)
            {
                var mess = string.Format(
                    "Превышено максимальное кол-во желаемых единиц продукта: max = {0}, желаемое = {1}",
                    ApiSettings.MaxWishListItemProductsQuantity,
                    quantity);
                throw new OperationException(ResultCodes.QUANTITY_OVERFLOW, mess);
            }
        }
    }
}