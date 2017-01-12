namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Базовый класс инкапсулирующий результаты выполнения некой операции.
	/// </summary>
	[DataContract]
	public class ResultBase
	{
		/// <summary>
		/// Код возврата. Допустимые значение определяются операцией, но значения уникальны в рамках компонента.
		/// Предопределенные значения: 0 - Успешное выполение. 1 - Неизвестная ошибка.
		/// </summary>
		[DataMember]
		public int ResultCode { get; set; }

		/// <summary>
		/// Признак успешного выполнения операции.
		/// </summary>
		[DataMember]
		public bool Success 
		{
			get
			{
				return this.ResultCode == (int)ResultCodes.Success;
			}

// ReSharper disable ValueParameterNotUsed
			set
// ReSharper restore ValueParameterNotUsed
			{
			}
		}

		/// <summary>
		/// Обязательно заполняется, если код возврата не равен 0 (часто значение получаемое с помощью выражения exception.ToString()).
		/// В других случаях никогда не заполняется.
		/// </summary>
		[DataMember]
		public string ResultDescription { get; set; }
	}
}