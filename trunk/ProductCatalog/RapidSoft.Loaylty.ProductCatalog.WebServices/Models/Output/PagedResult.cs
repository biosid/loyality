using System.Runtime.Serialization;

namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output
{
    [DataContract]
    public class PagedResult<T> : ArrayResult<T>
    {
        [DataMember]
        public int? TotalCount { get; set; }
    }
}
