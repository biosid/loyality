namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Объект-результат проверки заказа
	/// </summary>
	[DataContract]
	public class CheckOrderResult : ResultBase
	{
		/// <summary>
		/// Факт возможности выполнения заказа. Принимает одно из следующих значений:
		/// 0 – заказ не может быть выполнен (отказ);
		/// 1 – заказ может быть выполнен.
		/// </summary>
		[DataMember]
		public int Checked
		{
			get;
			set;
		}

		/// <summary>
		/// Текстовое описание причины отказа. Описание предназначено для отображения пользователю. Заполняется, если Checked = 0.
		/// </summary>
		[DataMember]
		public string Reason
		{
			get;
			set;
		}

		/// <summary>
		/// Код причины отказа в информационной системе поставщика. Код предназначен для логирования, сбора статистики и разбора. Не обязателен к заполнению.
		/// </summary>
		[DataMember]
		public string ReasonCode
		{
			get;
			set;
		}

		public static CheckOrderResult BuildSuccess(int @checked, string reason, string reasonCode)
		{
			return new CheckOrderResult
				       {
					       Checked = @checked,
					       Reason = reason,
					       ReasonCode = reasonCode,
					       ResultCode = (int)ResultCodes.Success,
					       ResultDescription = null
				       };
		}

		public static CheckOrderResult BuildFail(ResultCodes code, string description)
		{
			return new CheckOrderResult
				       {
						   Checked = 0,
					       Reason = null,
					       ReasonCode = null,
					       ResultCode = (int)code,
					       ResultDescription = description
				       };
		}
	}
}