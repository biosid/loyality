namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CalculatedProductPrices
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public decimal PriceRur { get; set; }

        [DataMember]
        public decimal PriceBase { get; set; }

        [DataMember]
        public decimal PricesAction { get; set; }
    }
}
