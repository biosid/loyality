namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	public enum OrderStatuses
	{
		/// <summary>
		/// Создан запрос на заказ
		/// </summary>
		Draft = 0,

		/// <summary>
		/// Оформление
		/// </summary>
		Registration = 5,

		/// <summary>
		/// В обработке
		/// </summary>
		Processing = 10,

		/// <summary>
		/// Аннулирован партнером
		/// </summary>
		CancelledByPartner = 20,

		/// <summary>
		/// Требует доставки
		/// </summary>
		DeliveryWaiting = 30,

		/// <summary>
		/// Доставка
		/// </summary>
		Delivery = 40,

		/// <summary>
		/// Доставлен
		/// </summary>
		Delivered = 50,

		/// <summary>
		/// Доставлен c задержкой по вине партнёра
		/// </summary>
		DeliveredWithDelay = 51,

        /// <summary>
        /// Не доставлен
        /// </summary>
        NotDelivered = 60,
	}
}