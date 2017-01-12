namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Статусы партнера.
    /// </summary>
    [DataContract]
    public enum PartnerThrustLevel
    {
        [EnumMember]
        Low = 0,

        [EnumMember]
        Middle = 1,

        [EnumMember]
        High = 2,
    }
}