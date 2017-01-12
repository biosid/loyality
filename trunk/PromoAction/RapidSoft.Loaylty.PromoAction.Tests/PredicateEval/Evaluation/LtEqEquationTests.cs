namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class LtEqEquationTests
	{
		private readonly IEvalStrategySelector factory = new EvalStrategySelector(new VariableResolver(null, TestHelper2.Aliases));
		[TestMethod]
		public void ShouldReturnTrueForEqual()
		{
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, TestHelper2.Numeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.String, TestHelper2.String, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.True, TestHelper2.True, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, TestHelper2.DateTimeNow, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsTrue(new LtEqEquation(TH.BuildTwoValue(TH.dateTimeRange5, TH.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnTrueForLow()
		{
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, TestHelper2.OtherNumeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.String, TestHelper2.OtherString, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.False, TestHelper2.True, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsTrue(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, TestHelper2.DateTimeNowPlus5Days, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsTrue(new LtEqEquation(TH.BuildTwoValue(TH.dateTimeRange4, TH.dateTimeRange5, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnSql()
		{
			var values1 = TestHelper2.BuildTwoValueWithAttr(TestHelper2.Numeric, valueType.numeric, aliases: TestHelper2.Aliases);
			var result1 = new LtEqEquation(values1, this.factory).EvaluateExt();

			Assert.AreEqual(result1.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result1), "table.column<=" + TestHelper2.Numeric);

			var values2 = TestHelper2.BuildTwoValueWithAttr(TestHelper2.String, valueType.@string, aliases: TestHelper2.Aliases);
			var result2 = new LtEqEquation(values2, this.factory).EvaluateExt();
			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result2), "table.column<='" + TestHelper2.String + "'");

			var values3 = TestHelper2.BuildTwoValueWithAttr(TestHelper2.True, valueType.boolean, aliases: TestHelper2.Aliases);
			var result3 = new LtEqEquation(values3, this.factory).EvaluateExt();
			Assert.AreEqual(result3.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result3), "table.column<=1");

			var values4 = TestHelper2.BuildTwoValueWithAttr(new DateTime(2012, 1, 2, 5, 8, 9).ToString(CultureInfo.InvariantCulture), valueType.datetime, aliases: TestHelper2.Aliases);
			var result4 = new LtEqEquation(values4, this.factory).EvaluateExt();
			Assert.AreEqual(result4.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result4), "table.column<=CONVERT(datetime,'2012-01-02T05:08:09.000',126)");
		}

		[TestMethod]
		public void ShouldReturnFalse()
		{
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.OtherNumeric, TestHelper2.Numeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.OtherString, TestHelper2.String, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.True, TestHelper2.False, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNowPlus5Days, TestHelper2.DateTimeNow, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEqEquation(TH.BuildTwoValue(TH.dateTimeRange5, TH.dateTimeRange4, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnFalseWhenFirstOperandNull()
		{
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(null, TestHelper2.OtherNumeric, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(null, TestHelper2.OtherString, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(null, TestHelper2.False, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(null, TestHelper2.DateTimeNowPlus5Days, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEqEquation(TH.BuildTwoValue(null, TH.dateTimeRange4, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnFalseWhenSecondOperandNull()
		{
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.Numeric, null, valueType.numeric, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.String, null, valueType.@string, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.True, null, valueType.boolean, context: TestHelper2.EmptyDic), this.factory).Evaluate());
			Assert.IsFalse(new LtEqEquation(TestHelper2.BuildTwoValue(TestHelper2.DateTimeNow, null, valueType.datetime, context: TestHelper2.EmptyDic), this.factory).Evaluate());

			// NOTE: GUI элемент управления не поддерживает тип данных "datetime-range"
//			Assert.IsFalse(new LtEqEquation(TH.BuildTwoValue(TH.dateTimeRange5, null, valueType.datetimerange), TestHelper2.EmptyDic, factory).Evaluate());
		}
	}
}
