namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[XmlRoot(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public partial class CommitOrderMessage
	{
		[Required, ValidateObject]
		public Order Order { get; set; }
	}
}