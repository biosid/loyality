namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    using System.Xml.Serialization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[XmlRoot(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public partial class CommitOrdersMessage
	{
		[XmlArrayItem("Order", IsNullable = false)]
		public Order[] Orders
		{
		    get;
		    set;
		}
	}
}