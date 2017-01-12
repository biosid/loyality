namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using PartnersConnector.Interfaces.Entities;

    [XmlRootAttribute(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
    public class GetDeliveryVariantsMessage
    {
        [ValidateObject]
        public string ClientId { get; set; }

        [Required, ValidateObject]
        public Location Location { get; set; }

        [XmlArrayItem("Item", IsNullable = false)]
        [Required, ValidateObject]
        public OrderItem[] Items { get; set; }
    }
}