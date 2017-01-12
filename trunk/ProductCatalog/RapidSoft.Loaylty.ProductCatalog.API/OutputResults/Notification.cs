namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Notification
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int ProductQuantity { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public decimal ItemBonusCost { get; set; }

        [DataMember]
        public decimal TotalBonusCost { get; set; }
    }
}