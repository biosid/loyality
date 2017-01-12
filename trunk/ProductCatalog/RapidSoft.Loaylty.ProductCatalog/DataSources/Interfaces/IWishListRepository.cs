namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    using WishListItem = RapidSoft.Loaylty.ProductCatalog.API.Entities.WishListItem;

    /// <summary>
    /// Интерфейс репозитория управления WishList клиента.
    /// </summary>
    public interface IWishListRepository
    {
        WishListItem Get(string productId, string clientId);

        int GetWishListCount(string userId);

        IList<WishListItem> GetWishList(string userId);

        IList<WishListItem> GetWishList(
            string userId, WishListSortTypes sortType, SortDirections sortDirect, int? skip = null, int? take = null);

        int GetCountByClientId(string clientId);

        WishListItem CreateOrUpdate(string productId, string clientId, int quantity = 1);

        WishListItem IncreaseQuantityOrCreate(string productId, string clientId, int quantity = 1);

        bool Remove(string productId, string userId);

        Page<Notification> GetItemsToNotify(string clientId, int? take = null, bool? calcTotal = null);

        void CleanUpNonSentNotifications();

        List<WishListItemNotification> GetWishListToNotify();

        void AddNotifications(List<WishListItemNotification> notifications);
    }
}
