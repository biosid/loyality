namespace RapidSoft.Loaylty.PartnersConnector.Tests.DTO
{
	using System.Diagnostics.CodeAnalysis;
	using System.Text;

	using Common.DTO.CheckOrder;

	using Logging;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
		Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class CheckOrderResultTests
    {
        private readonly ILog log = LogManager.GetLogger(typeof (CheckOrderResultTests));

		[TestMethod]
		public void ShouldSerialize()
		{
			var orderResult1 = new CheckOrderResult { Checked = 1 };
			var orderResult2 = new CheckOrderResult
				                   {
					                   Checked = 0,
					                   Reason = "Доставка в указанный город не осуществляется в зимнее время года"
				                   };

			var str = orderResult1.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
            log.Debug(str);
			str = orderResult2.Serialize(Encoding.UTF8);
			Assert.IsNotNull(str);
            log.Debug(str);
		}

		[TestMethod]
		public void ShouldDeserialize()
		{
			const string Str1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CheckOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Checked>1</Checked>
</CheckOrderResult>";
			const string Str2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CheckOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Checked>0</Checked>
  <Reason>Доставка в указанный город не осуществляется в зимнее время года</Reason>
  <ReasonCode>ReasonCode</ReasonCode>
</CheckOrderResult>";
			var result = Str1.Deserialize<CheckOrderResult>(Encoding.UTF8);
			Assert.AreEqual(result.Checked, 1);
			Assert.IsNull(result.Reason);
			Assert.IsNull(result.ReasonCode);

			result = Str2.Deserialize<CheckOrderResult>(Encoding.UTF8);
			Assert.AreEqual(result.Checked, 0);
			Assert.AreEqual(result.Reason, "Доставка в указанный город не осуществляется в зимнее время года");
			Assert.AreEqual(result.ReasonCode, "ReasonCode");
		}
	}
}