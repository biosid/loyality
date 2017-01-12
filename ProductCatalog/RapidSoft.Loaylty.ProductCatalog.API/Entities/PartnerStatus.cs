namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Статусы партнера.
    /// </summary>
    [DataContract]
    public enum PartnerStatus
    {
        [EnumMember]
        NotActive = 0,

        [EnumMember]
        Active = 1,
    }
}