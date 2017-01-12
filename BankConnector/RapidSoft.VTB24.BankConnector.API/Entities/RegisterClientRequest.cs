namespace RapidSoft.VTB24.BankConnector.API.Entities
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Runtime.Serialization;
	using System.Text.RegularExpressions;

	using RapidSoft.VTB24.BankConnector.DataModels;

	[DataContract(Namespace = Globals.DefaultNamespace)]
	public class RegisterClientRequest
	{
		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public string MiddleName { get; set; }

		[DataMember]
		public string MobilePhone { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public Gender Gender { get; set; }

		[DataMember]
		public DateTime BirthDate { get; set; }

		public void Validate()
		{
			Contract.Requires(
				this.BirthDate.Year >= DateTime.Now.Year - 100 && this.BirthDate.Year <= DateTime.Now.Year - 18);
			Contract.Requires(this.FirstName != null);
			Contract.Requires(this.FirstName.Length <= 50);
			Contract.Requires(this.LastName != null);
			Contract.Requires(this.LastName.Length <= 50);
			Contract.Requires(this.MiddleName == null || this.MiddleName.Length <= 50);
			Contract.Requires(this.MobilePhone != null);
			Contract.Requires(this.MobilePhone.Length <= 20);
			Contract.Requires(Regex.IsMatch(this.MobilePhone, "^\\+?[0-9]+$"));
			Contract.Requires(this.Email == null || this.Email.Length <= 255);
		}
	}
}
