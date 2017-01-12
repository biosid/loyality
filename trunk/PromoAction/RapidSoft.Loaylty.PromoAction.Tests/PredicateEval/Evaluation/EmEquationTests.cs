namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class EmEquationTests
	{
		private readonly IEvalStrategySelector factory = new EvalStrategySelector(new VariableResolver(null, TestHelper2.Aliases));

		[TestMethod]
		public void ShouldReturnTrue()
		{
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue(null, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue(null, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue(string.Empty, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue("           ", valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue(null, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new EmEquation(TestHelper2.BuildOneValue(null, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnSql()
		{
			var values = TestHelper2.BuildOneValueWithAttr(TestHelper2.Numeric, valueType.numeric, aliases: TestHelper2.Aliases);
			var result = new EmEquation(values, this.factory).EvaluateExt();

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result), "table.column IS NULL");
		}

		[TestMethod]
		public void ShouldReturnFalse()
		{
			Assert.IsFalse(new EmEquation(TestHelper2.BuildOneValue(TestHelper2.Numeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new EmEquation(TestHelper2.BuildOneValue(TestHelper2.String, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new EmEquation(TestHelper2.BuildOneValue(TestHelper2.True, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new EmEquation(TestHelper2.BuildOneValue(TestHelper2.DateTimeNow, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new EmEquation(TestHelper2.BuildOneValue(TestHelper2.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}
	}
}