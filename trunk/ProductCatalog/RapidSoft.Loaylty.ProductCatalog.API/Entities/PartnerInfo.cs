namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    using RapidSoft.Extensions;

    [DataContract]
    public class PartnerInfo
    {
        public PartnerInfo()
        {
        }

        public PartnerInfo(Partner partner)
        {
            partner.ThrowIfNull("partner");

            this.Id = partner.Id;
            this.Name = partner.Name;
            this.Type = partner.Type;
            this.IsCarrier = partner.IsCarrier;
            this.Status = partner.Status;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public PartnerType Type { get; set; }

        [DataMember]
        public bool IsCarrier { get; set; }

        [DataMember]
        public PartnerStatus Status { get; set; }
    }
}