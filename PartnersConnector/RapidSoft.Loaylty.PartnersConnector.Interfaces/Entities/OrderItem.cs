namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	[Serializable]
	public class OrderItem
	{
		/// <summary>
		/// Идентификатор товара в информационной системе поставщика, полученный из каталога вознаграждений (см. атрибут offer/@id в описании формата YML).
		/// </summary>
		[DataMember]
		public string OfferId { get; set; }

		/// <summary>
		/// Наименование товара в информационной системе поставщика, полученное из каталога вознаграждений (см. элемент offer/name в описании формата YML).
		/// </summary>
		[DataMember]
		public string OfferName { get; set; }

		/// <summary>
		/// Цена вознаграждения, полученная из каталога вознаграждений (см. элемент offer/priceв описании формата YML).
		/// </summary>
		[DataMember]
		public decimal Price { get; set; }

		[DataMember]
		public int Weight { get; set; }

		/// <summary>
		/// Количество заказанных экземпляров товара
		/// </summary>
		[DataMember]
		public int Amount { get; set; }

		/// <summary>
		/// Идентификатор элемента корзины заполняется в том случае, если товар присутствовал и был заказан с использованием корзины.
		/// </summary>
		[DataMember]
		public string BasketItemId { get; set; }
	}
}