namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum NotAvailableCause
    {
        [EnumMember]
        No = 0,

        [EnumMember]
        DeactivatedCategory = 1
    }
}