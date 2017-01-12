namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.CommitOrder
{
    using System.ComponentModel.DataAnnotations;

    /// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	public partial class Contact
	{
        [Required]
        public string FirstName { get; set; }

		/// <remarks/>
        public string MiddleName { get; set; }

		/// <remarks/>
        public string LastName { get; set; }

        [Required]
        public long? PhoneNumber { get; set; }

		/// <remarks/>
        public string Email { get; set; }
	}
}