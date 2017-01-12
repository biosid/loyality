namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.WishList;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.WishList.Input;

    /// <summary>
    /// Интерфейс управления WishList клиента.
    /// </summary>
    [ServiceContract]
    public interface IWishListService : ISupportService
    {
        /// <summary>
        /// Добавление товара в WishList.
        /// </summary>
        [OperationContract]
        ResultBase Add(AddParameters parameters);

        /// <summary>
        /// Изменение количества товаров в WishList клиента.
        /// </summary>
        [OperationContract]
        ResultBase SetQuantity(SetQuantityParameters parameters);

        /// <summary>
        /// Удаление товара из WishList клиента.
        /// </summary>
        [OperationContract]
        ResultBase Remove(RemoveParameters parameters);

        /// <summary>
        /// Получение элемента WishList клиента.
        /// </summary>
        [OperationContract]
        ValueResult<ClientItem> GetItem(GetItemParameters parameters);

        /// <summary>
        /// Получение перечня товаров из WishList клиента.
        /// </summary>
        [OperationContract]
        PagedResult<ClientItem> Get(GetParameters parameters);

        [OperationContract]
        ResultBase MakeNotifications();

        [OperationContract]
        PagedResult<WishListNotification> GetNotifications(GetNotificationsParameters parameters);
    }
}
