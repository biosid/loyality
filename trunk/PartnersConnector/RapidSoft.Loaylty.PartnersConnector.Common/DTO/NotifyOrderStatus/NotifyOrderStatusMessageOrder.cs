namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.NotifyOrderStatus
{
    using System.ComponentModel.DataAnnotations;

    using RapidSoft.Loaylty.PartnersConnector.Common.Services.Validation;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLSchema.xsd")]
	public class NotifyOrderStatusMessageOrder
	{
		private string internalOrderIdField;

		private int? statusCodeField;

		private System.DateTime? statusDateTimeField;

		private string statusReasonField;

		private string internalStatusCodeField;

        [Required]
		public string InternalOrderId
		{
			get
			{
				return this.internalOrderIdField;
			}
			set
			{
				this.internalOrderIdField = value;
			}
		}

        [Required]
        [OfflineOrderStatus]
		public int? StatusCode
		{
			get
			{
				return this.statusCodeField;
			}
			set
			{
				this.statusCodeField = value;
			}
		}

        [Required]
		public System.DateTime? StatusDateTime
		{
			get
			{
				return this.statusDateTimeField;
			}
			set
			{
				this.statusDateTimeField = value;
			}
		}

		public string StatusReason
		{
			get
			{
				return this.statusReasonField;
			}
			set
			{
				this.statusReasonField = value;
			}
		}

		public string InternalStatusCode
		{
			get
			{
				return this.internalStatusCodeField;
			}
			set
			{
				this.internalStatusCodeField = value;
			}
		}


        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientId { get; set; }
	}
}