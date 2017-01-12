namespace RapidSoft.Loaylty.PartnersConnector.Tests.DTO
{
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Text;

	using Common.DTO.CommitOrder;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;

	using RapidSoft.Loaylty.Logging;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class CommitOrdersResultTests
	{
        private readonly ILog log = LogManager.GetLogger(typeof(CommitOrdersResultTests));

		[TestMethod]
		public void ShouldSerializeOneOrder()
		{
			var orderResult1 = new CommitOrderResult { Confirmed = 1, InternalOrderId = "X100500" };
			var orderResult2 = new CommitOrderResult
								   {
									   Confirmed = 0,
									   InternalOrderId = "X100500",
									   Reason =
										   "Доставка в указанный город не осуществляется в зимнее время года"
								   };

			var str = orderResult1.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
			str = orderResult2.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
		}

		[TestMethod]
		public void ShouldDeserializeOneOrder()
		{
			const string Str1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <InternalOrderId>X100500</InternalOrderId>
  <Confirmed>1</Confirmed>
</CommitOrderResult>";
			const string Str2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <InternalOrderId>X100500</InternalOrderId>
  <Confirmed>0</Confirmed>
<Reason>Доставка в указанный город не осуществляется в зимнее время года</Reason>
</CommitOrderResult>";
			var result = Str1.Deserialize<CommitOrderResult>(Encoding.UTF8);
			Assert.IsNull(result.OrderId);
			Assert.AreEqual(result.InternalOrderId, "X100500");
			Assert.AreEqual(result.Confirmed, 1);
			Assert.IsNull(result.Reason);
			Assert.IsNull(result.ReasonCode);

			result = Str2.Deserialize<CommitOrderResult>(Encoding.UTF8);
			Assert.IsNull(result.OrderId);
			Assert.AreEqual(result.InternalOrderId, "X100500");
			Assert.AreEqual(result.Confirmed, 0);
			Assert.AreEqual(result.Reason, "Доставка в указанный город не осуществляется в зимнее время года");
			Assert.IsNull(result.ReasonCode);
		}

		[TestMethod]
		public void ShouldSerializeOrdersBatch()
		{
			var results = new CommitOrdersResult
							  {
								  Orders =
									  new[]
										  {
											  new CommitOrderResult
												  {
													  OrderId = "ABC123",
													  InternalOrderId = "X100500",
													  Confirmed = 1
												  },
											  new CommitOrderResult
												  {
													  OrderId = "ABC125",
													  InternalOrderId = "X100700",
													  Confirmed = 0,
													  Reason =
														  "Доставка в указанный город не осуществляется в зимнее время года"
												  }
										  }
							  };

			var str = results.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
			log.Debug(str);
		}

		[TestMethod]
		public void ShouldDeserializeOrdersBatch()
		{
			const string Str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrdersResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Orders>
	<Order>
	  <OrderId>ABC123</OrderId>
	  <InternalOrderId>X100500</InternalOrderId>
	  <Confirmed>1</Confirmed>
	</Order>
	<Order>
	  <OrderId>ABC125</OrderId>
	  <InternalOrderId>X100700</InternalOrderId>
	  <Confirmed>0</Confirmed>
	  <Reason>Доставка в указанный город не осуществляется в зимнее время года</Reason>
	</Order>
  </Orders>
</CommitOrdersResult>";
			var result = Str.Deserialize<CommitOrdersResult>(Encoding.UTF8);
			Assert.AreEqual(result.Orders.Length, 2);
			Assert.IsTrue(result.Orders.Any(x => x.OrderId == "ABC123"));
			Assert.IsTrue(result.Orders.Any(x => x.InternalOrderId == "X100700"));
		}
	}
}
