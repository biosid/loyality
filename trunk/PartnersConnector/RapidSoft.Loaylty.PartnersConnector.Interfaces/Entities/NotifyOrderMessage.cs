namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System;

	[Serializable]
	public class NotifyOrderMessage
	{
        /// <summary>
        /// Идентификатор партнёра.
        /// </summary>
        public int PartnerId { get; set; }

		/// <summary>
		/// Идентификатор заказа в информационной системе поставщика.
		/// </summary>
		public string PartnerOrderId { get; set; }

		/// <summary>
		/// Код статуса
		/// </summary>
		public OrderStatuses Code { get; set; }

		/// <summary>
		/// Дата и время изменения статуса в информационной системе поставщика.
		/// </summary>
		public DateTime StatusDateTime { get; set; }

		/// <summary>
		/// Причина изменения статуса.
		/// </summary>
		public string StatusReason { get; set; }

		/// <summary>
		/// Код статуса в информационной системе поставщика. Предназначен для сверок.
		/// </summary>
		public string PartnerCode { get; set; }

        public string ClientId { get; set; }
	}
}