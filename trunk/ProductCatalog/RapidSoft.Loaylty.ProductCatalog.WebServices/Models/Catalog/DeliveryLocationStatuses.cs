namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum DeliveryLocationStatuses
    {
        [EnumMember]
        CorrectBinded = 1,

        [EnumMember]
        NotBinded = 2,

        [EnumMember]
        NotCorrectKladr = 3,

        [EnumMember]
        KladrDuplication = 4
    }
}
