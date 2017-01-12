namespace RapidSoft.Loaylty.PartnersConnector.Tests.DTO
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;

	using Common.DTO.CommitOrder;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;

	using RapidSoft.Loaylty.Logging;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class CommitOrdersMessageTests
	{
        private readonly ILog log = LogManager.GetLogger(typeof(CommitOrdersMessageTests));

		[TestMethod]
		public void ShouldSerializeOneOrder()
		{
			var orderItem = new OrderItem { OfferId = "101", OfferName = "Утюг", Price = 800, Amount = 3 };
			var contact = new Contact { FirstName = "Иван", PhoneNumber = 89101234567 };
			var timeFrom = new DateTime(1, 1, 1, 08, 00, 00);
			var timeTo = new DateTime(1, 1, 1, 12, 30, 00);
			var deliveryInfo = new DeliveryInfo
				                   {
					                   CountryCode = "RU",
					                   CountryName = "Россия",
					                   PostCode = "123456",
					                   Address = "Московская обл., г. Ногинск, ул. Ленина, д. 987, кв. 654",
					                   Comment = "Код от домофона: 654-вызов-777",
					                   Contacts = new[] { contact }
				                   };

			var message = new CommitOrderMessage
				              {
					              Order =
						              new Order
							              {
								              OrderId = "ABC123",
								              ItemsCost = 2400,
								              DeliveryCost = 300,
								              TotalCost = 2700,
								              Items = new[] { orderItem },
								              DeliveryInfo = deliveryInfo
							              }
				              };

			var str = message.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
		}

		[TestMethod]
		public void ShouldSerializeOneOrder2()
		{
			var orderItem = new OrderItem { OfferId = "101", OfferName = "Утюг", Price = 800, Amount = 3 };
			var contact = new Contact
				              {
									FirstName = "Иван", 
									MiddleName = null,
									LastName = null, 
									PhoneNumber = 89101234567,
									Email = null
				              };
			var deliveryInfo = new DeliveryInfo
				                   {
					                   CountryCode = "RU",
					                   CountryName = "Россия",
					                   PostCode = "123456",
					                   Address = "Московская обл., г. Ногинск, ул. Ленина, д. 987, кв. 654",					                   
					                   Comment = null,
					                   Contacts = new[] { contact }
				                   };

			var message = new CommitOrderMessage
			{
				Order =
					new Order
					{
						OrderId = "ABC123",
						ItemsCost = 2400,
						DeliveryCost = 300,
						TotalCost = 2700,
						Items = new[] { orderItem },
						DeliveryInfo = deliveryInfo
					}
			};

			var str = message.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
		}

		[TestMethod]
		public void ShouldSerializeOrdersBatch()
		{
			var orderItem1 = new OrderItem { OfferId = "101", OfferName = "Утюг", Price = 800, Amount = 3 };
			var contact1 = new Contact { FirstName = "Иван", PhoneNumber = 89101234567 };
			var timeFrom1 = new DateTime(1, 1, 1, 08, 00, 00);
			var timeTo1 = new DateTime(1, 1, 1, 12, 30, 00);
			var deliveryInfo1 = new DeliveryInfo
				                   {
					                   CountryCode = "RU",
					                   CountryName = "Россия",
					                   PostCode = "123456",
					                   Address = "Московская обл., г. Ногинск, ул. Ленина, д. 987, кв. 654",
					                   Comment = "Код от домофона: 654-вызов-777",
					                   Contacts = new[] { contact1 }
				                   };
			var order1 = new Order
				             {
					             OrderId = "ABC123",
					             ItemsCost = 2400,
					             DeliveryCost = 300,
					             TotalCost = 2700,
					             Items = new[] { orderItem1 },
					             DeliveryInfo = deliveryInfo1
				             };

			var orderItem2 = new OrderItem { OfferId = "213", OfferName = "Радиоприемник", Price = 500, Amount = 1 };
			var contact2 = new Contact { FirstName = "Федор", PhoneNumber = 89107654321 };
			var timeFrom2 = new DateTime(1, 1, 1, 09, 00, 00);
			var timeTo2 = new DateTime(1, 1, 1, 13, 00, 00);
			var deliveryInfo2 = new DeliveryInfo
				                   {
					                   CountryCode = "RU",
					                   CountryName = "Россия",
					                   PostCode = "654321",
					                   Address = "Сахалин, ул. Ленина, д. 1",
					                   Comment = null,
					                   Contacts = new[] { contact2 }
				                   };
			var order2 = new Order
				             {
					             OrderId = "ABC125",
					             ItemsCost = 500,
					             DeliveryCost = 200,
					             TotalCost = 700,
					             Items = new[] { orderItem2 },
					             DeliveryInfo = deliveryInfo2
				             };

			var ordersMessage = new CommitOrdersMessage { Orders = new[] { order1, order2 } };

			var str = ordersMessage.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
		}
	}
}
