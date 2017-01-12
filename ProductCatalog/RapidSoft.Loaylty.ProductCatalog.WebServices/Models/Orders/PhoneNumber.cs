namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PhoneNumber
    {
        [DataMember]
        public string LocalNumber { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string CountryCode { get; set; }
    }
}
