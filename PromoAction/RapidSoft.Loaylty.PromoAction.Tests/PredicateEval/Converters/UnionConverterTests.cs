namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Converters
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class UnionConverterTests
	{
		private const string Value1 = "5";

		private const string Value2 = "table.column";

		[TestMethod]
		public void ShouldConvertTwoOperands()
		{
			var innerConverter1 = new TwoOperandEquationConverter(Value1, "=", Value2);
			var innerConverter2 = new TwoOperandEquationConverter(Value2, "<", Value1);
			var evalResults = new[]
				                  {
					                  EvaluateResult.BuildTansformed(innerConverter1), EvaluateResult.BuildTansformed(innerConverter2)
				                  };
			var converter = new UnionConverter(" OR ", evalResults);

			Assert.AreEqual(TestHelper2.Convert(converter), "(5=table.column OR table.column<5)");
		}

		[TestMethod]
		public void ShouldConverTwoOperandsWithInnerOr()
		{
			var innerConverter1 = new TwoOperandEquationConverter(Value1, "=", Value2);
			var innerConverter2 = new TwoOperandEquationConverter(Value2, "=", Value1);
			var converter3 = new OneOperandEquationConverter(Value2, " IS NULL");
			var andEvalConverter = new[]
				                  {
					                  EvaluateResult.BuildTansformed(innerConverter1), EvaluateResult.BuildTansformed(innerConverter2)
				                  };
			var andConverter = new UnionConverter(" OR ", andEvalConverter);

			var evalResult = new[] { EvaluateResult.BuildTansformed(andConverter), EvaluateResult.BuildTansformed(converter3) };

			var converter = new UnionConverter(" AND ", evalResult);

			Assert.AreEqual(TestHelper2.Convert(converter), "((5=table.column OR table.column=5) AND table.column IS NULL)");
		}

		[TestMethod]
		public void ShouldConvertOneOperand()
		{
			var innerConverter1 = new TwoOperandEquationConverter(Value1, "=", Value2);
			var evalResults = new[]
				                  {
					                  EvaluateResult.BuildTansformed(innerConverter1)
				                  };
			var converter = new UnionConverter(" OR ", evalResults);

			Assert.AreEqual(TestHelper2.Convert(converter), "5=table.column");
		}
	}
}
