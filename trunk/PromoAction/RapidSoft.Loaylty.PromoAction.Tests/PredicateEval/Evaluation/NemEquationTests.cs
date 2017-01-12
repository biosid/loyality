namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class NemEquationTests
	{
		private readonly IEvalStrategySelector factory = new EvalStrategySelector(new VariableResolver(null, TestHelper2.Aliases));

		[TestMethod]
		public void ShouldReturnFalse()
		{
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue(null, valueType.numeric), this.factory).Evaluate());
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue(null, valueType.@string), this.factory).Evaluate());
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue(string.Empty, valueType.@string), this.factory).Evaluate());
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue("           ", valueType.@string), this.factory).Evaluate());
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue(null, valueType.boolean), this.factory).Evaluate());
			Assert.IsFalse(new NemEquation(TestHelper2.BuildOneValue(null, valueType.datetime), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
			// Assert.IsFalse(new NemEquation(TH.BuildOneValue(null, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnSql()
		{
			var values = TestHelper2.BuildOneValueWithAttr(null, valueType.numeric, aliases: TestHelper2.Aliases);
			var result = new NemEquation(values, this.factory).EvaluateExt();

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result), "table.column IS NOT NULL");
		}

		[TestMethod]
		public void ShouldReturnTrue()
		{
			Assert.IsTrue(new NemEquation(TestHelper2.BuildOneValue(TestHelper2.Numeric, valueType.numeric), this.factory).Evaluate());
			Assert.IsTrue(new NemEquation(TestHelper2.BuildOneValue(TestHelper2.String, valueType.@string), this.factory).Evaluate());
			Assert.IsTrue(new NemEquation(TestHelper2.BuildOneValue(TestHelper2.True, valueType.boolean), this.factory).Evaluate());
			Assert.IsTrue(new NemEquation(TestHelper2.BuildOneValue(TestHelper2.DateTimeNow, valueType.datetime), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
			// Assert.IsTrue(new NemEquation(TH.BuildOneValue(TH.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}
	}
}
