namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ValueResult<T> : ResultBase
    {
        [DataMember]
        public T Value { get; set; }
    }
}
