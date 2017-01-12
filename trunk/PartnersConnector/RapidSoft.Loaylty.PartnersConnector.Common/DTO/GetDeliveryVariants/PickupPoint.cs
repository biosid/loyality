namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Offline;

    public class PickupPoint
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ExternalPickupPointId { get; set; }

        [Required]
        public string DeliveryVariantName { get; set; }

        [Required]
        public string ExternalDeliveryVariantId { get; set; }

        [Required]
        public string Address { get; set; }

        [XmlArrayItem("Phone", typeof(string))]
        public string[] Phones { get; set; }

        [XmlArrayItem("OperatingHour", typeof(string))]
        public string[] OperatingHours { get; set; }

        public string Description { get; set; }

        [XmlIgnore]
        [Required]
        public decimal? ItemsCost { get; set; }

        [XmlElement("ItemsCost")]        
        public string ItemsCostString
        {
            get
            {
                return ItemsCost.GetStringFromPrice();
            }
            set
            {
                ItemsCost = value.GetPriceFromString();
            }
        }

        [Required]
        public decimal? DeliveryCost { get; set; }

        [Required]
        public decimal? TotalCost { get; set; }
    }
}