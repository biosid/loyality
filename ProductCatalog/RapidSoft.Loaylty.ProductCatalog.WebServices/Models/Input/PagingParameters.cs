namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PagingParameters
    {
        [DataMember]
        public int Skip { get; set; }

        [DataMember]
        public int Take { get; set; }

        [DataMember]
        public bool CalculateTotalCount { get; set; }
    }
}
