namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Статусы партнера.
    /// </summary>
    [DataContract]
    public enum PartnerType
    {
        [EnumMember]
        Online = 0,

        [EnumMember]
        Direct = 1,

        [EnumMember]
        Offline = 2,
    }
}