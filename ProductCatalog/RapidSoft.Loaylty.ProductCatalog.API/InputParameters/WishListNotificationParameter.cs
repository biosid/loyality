namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class WishListNotificationParameter
    {
        public string UserId
        {
            get;
            set;
        }

        public string[] ProductIds
        {
            get;
            set;
        }
    }
}
