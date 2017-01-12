namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum DeliveryLocationUpdateSources
    {
        [EnumMember]
        Unknow = 0,

        [EnumMember]
        Arm = 1,

        [EnumMember]
        Import = 2
    }
}
