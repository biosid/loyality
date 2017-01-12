namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class FixedPrice
    {
        [DataMember]
        public decimal PriceAction { get; set; }

        [DataMember]
        public DateTime FixDate { get; set; }
    }
}
