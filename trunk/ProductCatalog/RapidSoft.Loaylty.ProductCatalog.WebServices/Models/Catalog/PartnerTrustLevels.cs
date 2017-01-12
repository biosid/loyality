namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PartnerTrustLevels
    {
        [EnumMember]
        Low = 0,

        [EnumMember]
        Middle = 1,

        [EnumMember]
        High = 2,
    }
}
