namespace RapidSoft.Loaylty.PartnersConnector.Tests.DTO
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;

	using Common.DTO.CheckOrder;
	using Common.DTO.CommitOrder;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;

	using RapidSoft.Loaylty.Logging;

	using DeliveryInfo = Common.DTO.CheckOrder.DeliveryInfo;
	using Order = Common.DTO.CheckOrder.Order;
	using OrderItem = Common.DTO.CheckOrder.OrderItem;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class CheckOrderMessageTests
	{
        private readonly ILog log = LogManager.GetLogger(typeof(CheckOrderMessageTests));
        
        [TestMethod]
		public void ShouldSerializeOneOrder()
		{
			var orderItem = new OrderItem { OfferId = "101", OfferName = "Утюг", Price = 800, Amount = 3 };
			// var contact = new Contact { FirstName = "Иван", PhoneNumber = "89101234567" };
			var timeFrom = new DateTime(1, 1, 1, 08, 00, 00);
			var timeTo = new DateTime(1, 1, 1, 12, 30, 00);
			var deliveryInfo = new DeliveryInfo
								   {
									   CountryCode = "RU",
									   CountryName = "Россия",
									   PostCode = "123456",
									   Address = "Московская обл., г. Ногинск, ул. Ленина, д. 987, кв. 654",
									   Comment = "Код от домофона: 654-вызов-777"
								   };

			var message = new CheckOrderMessage
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
									   Comment = null
								   };

			var message = new CheckOrderMessage
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
	}
}