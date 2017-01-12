namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[XmlRoot(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public class CommitOrderResult
	{
		[XmlElement(IsNullable = true)]
		public string OrderId { get; set; }

		[Required]
		public string InternalOrderId { get; set; }

		/// <remarks/>
        [Required]
        public int? Confirmed { get; set; }

		/// <remarks/>
		[XmlElement(IsNullable = true)]
		public string Reason { get; set; }

		/// <remarks/>
		[XmlElement(IsNullable = true)]
		public string ReasonCode { get; set; }
	}
}
