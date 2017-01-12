namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.WishList.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}