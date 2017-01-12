namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum PartnerStatuses
    {
        [EnumMember]
        NotActive = 0,

        [EnumMember]
        Active = 1,
    }
}
