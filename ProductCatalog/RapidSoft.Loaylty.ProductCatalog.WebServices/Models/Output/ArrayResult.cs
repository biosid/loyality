namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ArrayResult<T> : ResultBase
    {
        [DataMember]
        public T[] Items { get; set; }
    }
}
