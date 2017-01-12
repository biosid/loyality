namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    /// <summary>
    /// Типы доставки
    /// </summary>
    public enum DeliveryTypes
    {
        /// <summary>
        /// Курьерская доставка
        /// </summary>
        Delivery = 0,

        /// <summary>
        /// Самовывоз
        /// </summary>
        Pickup = 1,

		/// <summary>
		/// Доставка по электронной почте
		/// </summary>
		Email = 2
    }
}