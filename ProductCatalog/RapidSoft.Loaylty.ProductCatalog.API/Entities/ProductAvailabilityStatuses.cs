namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ProductAvailabilityStatuses
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        Available = 1,

        [EnumMember]
        CategoryIsNotActive = 2,

        [EnumMember]
        PartnerIsNotActive = 3,

        [EnumMember]
        ProductIsNotActive = 4,

        [EnumMember]
        CategoryPermissionNotFound = 5,

        [EnumMember]
        DeliveryRateNotFound = 6,

        [EnumMember]
        TargetAudienceNotFound = 7,

        [EnumMember]
        ModerationNotApplied = 8,

        [EnumMember]
        PriceNotFound = 9
    }
}
