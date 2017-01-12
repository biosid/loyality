namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum WeightProcessTypes
    {
        [EnumMember]
        AcceptEmptyWeight = 0,

        [EnumMember]
        WeightRequired = 1,

        [EnumMember]
        DefaultWeight = 2
    }
}
