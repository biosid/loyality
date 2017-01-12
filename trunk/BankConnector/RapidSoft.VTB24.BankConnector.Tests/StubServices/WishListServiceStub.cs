namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService;

    public class WishListServiceStub : StubBase, IWishListService
    {
        public string Echo(string message)
        {
            throw new System.NotImplementedException();
        }

        public WishListResult Add(string clientId, string productId, int quantity, Dictionary<string, string> clientContext)
        {
            throw new System.NotImplementedException();
        }

        public WishListResult SetQuantity(string clientId, string productId, int quantity, Dictionary<string, string> clientContext)
        {
            throw new System.NotImplementedException();
        }

        public WishListResult Remove(string clientId, string productId)
        {
            throw new System.NotImplementedException();
        }

        public GetWishListItemResult GetWishListItem(string clientId, string productId, Dictionary<string, string> clientContext)
        {
            throw new System.NotImplementedException();
        }

        public GetWishListResult GetWishList(GetWishListParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public ResultBase MakeWishListNotifications()
        {
            return new ResultBase()
                   {
                       Success = true
                   };
        }

        public GetWishListNotificationsResult GetWishListNotifications(GetWishListNotificationsParameters parameters)
        {
            return new GetWishListNotificationsResult()
                   {
                       Success = true,
                       Notifications = new Notification[]{}
                   };
        }
    }
}