namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[Serializable()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	public partial class DeliveryInfo
	{
        public string ExternalLocationId { get; set; }

        [Required]
        public string ExternalDeliveryVariantId { get; set; }

        public string ExternalPickupPointId { get; set; }

        [Required]
		public string CountryCode { get; set; }

        [Required]
		public string CountryName { get; set; }

		public string PostCode { get; set; }

        [Required]
		public string Address { get; set; }

		public string Comment { get; set; }

        [Required, ValidateObject]
        public Contact[] Contacts { get; set; }
	}
}