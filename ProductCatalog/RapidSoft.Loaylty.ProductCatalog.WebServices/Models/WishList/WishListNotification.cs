namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.WishList
{
    using System.Runtime.Serialization;

    [DataContract]
    public class WishListNotification
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int ProductQuantity { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public decimal ItemPrice { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }
    }
}
