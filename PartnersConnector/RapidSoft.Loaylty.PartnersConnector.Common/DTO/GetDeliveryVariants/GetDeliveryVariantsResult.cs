namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
    public class GetDeliveryVariantsResult
    {
        [Required]
        public int? ResultCode
        {
            get;
            set;
        }

        [StringLength(1000)]
        public string Reason
        {
            get;
            set;
        }

        [Required, ValidateObject]
        public VariantsLocation Location
        {
            get;
            set;
        }

        [ValidateObject]
        public DeliveryGroup[] DeliveryGroups
        {
            get;
            set;
        }
    }
}