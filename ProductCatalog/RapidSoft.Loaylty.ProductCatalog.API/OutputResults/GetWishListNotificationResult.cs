namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [System.Obsolete("Use Notification", true)]
    [DataContract]
    public class GetWishListNotificationResult
    {
        [DataMember]
        public string ProductId
        {
            get;
            set;
        }

        [DataMember]
        public string ProductName
        {
            get;
            set;
        }

        [DataMember]
        public int ProductQuantity
        {
            get;
            set;
        }

        [DataMember]
        public string ClientId
        {
            get;
            set;
        }
    }
}
