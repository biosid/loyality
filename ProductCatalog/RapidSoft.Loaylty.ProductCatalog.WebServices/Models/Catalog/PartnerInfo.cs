namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PartnerInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public PartnerTypes Type { get; set; }

        [DataMember]
        public bool IsCarrier { get; set; }

        [DataMember]
        public PartnerStatuses Status { get; set; }
    }
}
