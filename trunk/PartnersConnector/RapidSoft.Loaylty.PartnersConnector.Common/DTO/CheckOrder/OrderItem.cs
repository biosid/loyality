namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CheckOrder
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Offline;

    /// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[Serializable()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	public partial class OrderItem
	{
        [Required]
		public string OfferId { get; set; }

		/// <remarks/>
        [Required]
        public string OfferName { get; set; }

        [XmlIgnore]
        public decimal? Price { get; set; }

        [XmlElement("Price")]
        [Required]
        public string PriceString
        {
            get
            {
                return Price.GetStringFromPrice();
            }
            set
            {
                Price = value.GetPriceFromString();
            }
        }

		[XmlElement(IsNullable = true)]
        public int? Weight { get; set; }

        [Required]
        public int Amount { get; set; }

        public string BasketItemId
        {
            get;
            set;
        }
	}
}