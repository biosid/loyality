namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ProductCategoryTypes
    {
        [EnumMember]
        Static = 0,

        [EnumMember]
        Online = 1
    }
}
