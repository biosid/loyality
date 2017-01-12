namespace RapidSoft.Loaylty.IntegrationTests.PartnersConnector
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLSchema.xsd", IsNullable = false)]
	public class CheckOrderResult
	{
		/// <remarks/>
		public int Checked { get; set; }

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
		public string Reason { get; set; }

		public bool ShouldSerializeReason()
		{
			return this.Reason != null;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
		public string ReasonCode { get; set; }

		public bool ShouldSerializeReasonCode()
		{
			return this.ReasonCode != null;
		}
	}
}