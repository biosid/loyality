namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.NotifyOrderStatus
{
    using System.ComponentModel.DataAnnotations;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public class NotifyOrderStatusMessage
	{
		private NotifyOrderStatusMessageOrder[] ordersField;

		[System.Xml.Serialization.XmlArrayItemAttribute("Order", IsNullable = false)]
		[Required]
        public NotifyOrderStatusMessageOrder[] Orders
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
