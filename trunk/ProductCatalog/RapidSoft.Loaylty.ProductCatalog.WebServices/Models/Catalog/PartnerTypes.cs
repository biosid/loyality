namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PartnerTypes
    {
        [EnumMember]
        Online = 0,

        [EnumMember]
        Direct = 1,

        [EnumMember]
        Offline = 2,
    }
}
