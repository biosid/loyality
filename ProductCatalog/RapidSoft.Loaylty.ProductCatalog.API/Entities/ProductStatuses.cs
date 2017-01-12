namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ProductStatuses
    {
        [EnumMember]
        NotActive = 0,

        [EnumMember]
        Active = 1
    }
}