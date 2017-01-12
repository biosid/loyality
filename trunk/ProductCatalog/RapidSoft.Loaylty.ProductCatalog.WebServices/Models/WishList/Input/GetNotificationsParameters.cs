namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.WishList.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetNotificationsParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public bool Rebuild { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}