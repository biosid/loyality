namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    using System.Runtime.Serialization;

    public class VariantsLocation
    {
        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public string KladrCode { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        [DataMember]
        public string ExternalLocationId { get; set; }
    }
}