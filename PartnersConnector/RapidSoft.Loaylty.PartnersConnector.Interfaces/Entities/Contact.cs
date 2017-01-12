namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Контактная информация
	/// </summary>
	[DataContract]
	[Serializable]
	public class Contact
	{
		/// <summary>
		/// Имя контактного лица
		/// </summary>
		[DataMember]
		public string FirstName
		{
			get;
			set;
		}

		/// <summary>
		/// Фамилия контактного лица
		/// </summary>
		[DataMember]
		public string LastName
		{
			get;
			set;
		}

		/// <summary>
		/// Отчество контактного лица
		/// </summary>
		[DataMember]
		public string MiddleName
		{
			get;
			set;
		}

		/// <summary>
		/// Телефон
		/// </summary>
		[DataMember]
		public long Phone
		{
			get;
			set;
		}

		/// <summary>
		/// Адрес электронной почты
		/// </summary>
		[DataMember]
		public string Email
		{
			get;
			set;
		}
	}
}