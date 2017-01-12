namespace RapidSoft.Loaylty.PromoAction.Tests.Converters
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class RuleSqlConverterTests
	{
		[TestMethod]
		public void ShouldRenderValidCase()
		{
			var predicateEval = new MockConverter("ПРЕДИКАТ");
			const decimal Then = 56.7m;
			const decimal Else = 0.7834m;

			var converter = new RuleSqlConverter(predicateEval, Then, Else);
			
			Assert.AreEqual(TestHelper2.Convert(converter), "CASE WHEN ПРЕДИКАТ THEN 56.7 ELSE 0.7834 END");
		}
	}
}
