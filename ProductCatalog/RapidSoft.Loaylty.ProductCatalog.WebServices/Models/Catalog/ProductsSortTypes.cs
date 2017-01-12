namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ProductsSortTypes
    {
        [EnumMember]
        ByNameAsc = 0,

        [EnumMember]
        ByNameDesc = 1,

        [EnumMember]
        ByPriceAscByNameAsc = 2,

        [EnumMember]
        ByPriceDescByNameAsc = 3,

        [EnumMember]
        ByInsertedDateDescByNameAsc = 4,

        [EnumMember]
        ByPartnerProductIdAsc = 5,

        [EnumMember]
        ByPopularityDesc = 6,

        [EnumMember]
        Random = 7,

        [EnumMember]
        Recommended = 8
    }
}
