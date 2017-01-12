namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    /// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public partial class CommitOrdersResult
	{

		private CommitOrderResult[] ordersField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("Order", IsNullable = false)]
		public CommitOrderResult[] Orders
		{
			get
			{
				return this.ordersField;
			}
			set
			{
				this.ordersField = value;
			}
		}
	}
}