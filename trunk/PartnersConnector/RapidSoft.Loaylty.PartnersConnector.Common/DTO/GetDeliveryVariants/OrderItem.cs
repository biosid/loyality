namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Offline;

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

		/// <remarks/>
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