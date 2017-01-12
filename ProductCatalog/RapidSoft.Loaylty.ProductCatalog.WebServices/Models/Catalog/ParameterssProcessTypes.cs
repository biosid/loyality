namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum ParameterssProcessTypes
    {
        [EnumMember]
        AcceptParamDuplicate = 0,

        [EnumMember]
        NotAcceptParamDuplicate = 1,
    }
}
