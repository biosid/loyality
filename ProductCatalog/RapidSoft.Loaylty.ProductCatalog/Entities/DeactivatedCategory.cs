namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeactivatedCategory
    {
        [DataMember]
        public int Id { get; set; }
    }
}
