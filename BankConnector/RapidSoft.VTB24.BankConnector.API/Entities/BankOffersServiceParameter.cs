namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class BankOffersServiceParameter
    {
        [DataMember]
        public int SkipCount { get; set; }

        [DataMember]
        public int TakeCount { get; set; }

        [DataMember]
        public bool CountTotal { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        [DataMember]
        public string Id { get; set; }
    }
}
