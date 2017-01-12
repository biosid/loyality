namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CheckOrder
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using CommitOrder;

    using Offline;

    /// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[Serializable()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	public partial class Order
	{
        [Required]
	    public string OrderId { get; set; }

        [Required]
	    public string ClientId { get; set; }

		public int? TotalWeight  { get; set; }

        [XmlIgnore]
        public decimal? ItemsCost { get; set; }

        [XmlElement("ItemsCost")]
        [Required]
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

        [XmlIgnore]
        public decimal? DeliveryCost { get; set; }

        [XmlElement("DeliveryCost")]
        [Required]
        public string DeliveryCostString
        {
            get
            {
                return DeliveryCost.GetStringFromPrice();
            }
            set
            {
                DeliveryCost = value.GetPriceFromString();
            }
        }

        [XmlIgnore]
        public decimal? TotalCost { get; set; }

        [XmlElement("TotalCost")]
        [Required]
        public string TotalCostString
        {
            get
            {
                return TotalCost.GetStringFromPrice();
            }
            set
            {
                TotalCost = value.GetPriceFromString();
            }
        }

		[XmlArrayItem("Item", IsNullable = false)]
        [Required, ValidateObject]
        public OrderItem[] Items { get; set; }

		[XmlElement(IsNullable = true)]
        [Required, ValidateObject]
        public DeliveryInfo DeliveryInfo { get; set; }
	}
}