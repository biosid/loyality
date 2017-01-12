namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class LtEquationTests
	{
		private readonly IEvalStrategySelector factory = new EvalStrategySelector(new VariableResolver(null, TestHelper2.Aliases));

		[TestMethod]
		public void ShouldReturnFalse()
		{
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, TestHelper2.Numeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.String, TestHelper2.String, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.True, TestHelper2.True, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, TestHelper2.DateTimeNow, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEquation(TH.BuildTwoValue(TH.dateTimeRange5, TH.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnTrue()
		{
			Assert.IsTrue(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, TestHelper2.OtherNumeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.String, TestHelper2.OtherString, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.False, TestHelper2.True, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, TestHelper2.DateTimeNowPlus5Days, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsTrue(new LtEquation(TH.BuildTwoValue(TH.dateTimeRange4, TH.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnSql()
		{
			var values = TestHelper2.BuildTwoValueWithAttr(TestHelper2.Numeric, valueType.numeric, aliases: TestHelper2.Aliases);
			var result = new LtEquation(values, this.factory).EvaluateExt();

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result), "table.column<" + TestHelper2.Numeric);
		}

		[TestMethod]
		public void ShouldReturnFalseWhenFirstOperandNull()
		{
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(null, TestHelper2.OtherNumeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(null, TestHelper2.OtherString, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(null, TestHelper2.False, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(null, TestHelper2.DateTimeNowPlus5Days, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEquation(TH.BuildTwoValue(null, TH.dateTimeRange4, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnFalseWhenSecondOperandNull()
		{
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, null, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.String, null, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.True, null, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, null, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEquation(TH.BuildTwoValue(TH.dateTimeRange5, null, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}
	}
}
