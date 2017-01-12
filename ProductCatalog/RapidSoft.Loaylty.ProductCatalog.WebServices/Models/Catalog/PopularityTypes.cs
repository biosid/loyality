namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PopularityTypes
    {
        [EnumMember]
        MostWished = 0,

        [EnumMember]
        MostOrdered = 1,

        [EnumMember]
        MostViewed = 2,
    }
}
