namespace RapidSoft.Loaylty.PromoAction.Tests.Aggregates
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class AggregateRuleResultTests
	{
		[TestMethod]
		public void ShouldConvertAdditionWithOneTrueRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Fac1 = 5.5m;
			var ruleResults = new[] { RuleResult.BuildTrue(RuleType, Fac1) };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, 5.5m);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "5.5");
		}

		[TestMethod]
		public void ShouldConvertAdditionWithTwoTrueRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal fac1 = 5.5m;
			const decimal fac2 = 3.3m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildTrue(RuleType, fac1), 
									  RuleResult.BuildTrue(RuleType, fac2)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, 8.8m);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "8.8");
		}

		[TestMethod]
		public void ShouldConvertAdditionWithThreeTrueRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Fac1 = 1.1m;
			const decimal Fac2 = 2.2m;
			const decimal Fac3 = 3.3m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildTrue(RuleType, Fac1), 
									  RuleResult.BuildTrue(RuleType, Fac2), 
									  RuleResult.BuildTrue(RuleType, Fac3)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, 6.6m);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "6.6");
		}

		[TestMethod]
		public void ShouldConvertAdditionWithOneConvertibleRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Fac1 = 5.5m;
			const decimal Fac2 = 2.2m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildConvertible(RuleTypes.Multiplication, new MockConverter("ПРЕДИКАТ"), Fac1),
					                  RuleResult.BuildTrue(RuleType, Fac2)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, Fac2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "2.2+(CASE WHEN ПРЕДИКАТ THEN 5.5 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldConvertAdditionWithTwoConvertibleRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Fac1 = 1.1m;
			const decimal Fac2 = 2.2m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildTrue(RuleType, Fac1),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), 5.5m),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), 3.5m),
					                  RuleResult.BuildTrue(RuleType, Fac2)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, Fac1 + Fac2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "3.3+(CASE WHEN ПРЕДИКАТ1 THEN 5.5 ELSE 0 END)+(CASE WHEN ПРЕДИКАТ2 THEN 3.5 ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldConvertAdditionWithTwoConvertibleAndFalseAndTrueRule()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Fac1 = 1.1m;
			const decimal Fac2 = 6.12m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildTrue(RuleType, Fac1),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), 5.5m),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), 3.5m),
					                  RuleResult.BuildFalse(RuleType), 
									  RuleResult.BuildTrue(RuleType, Fac2)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, Fac1 + Fac2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "7.22+(CASE WHEN ПРЕДИКАТ1 THEN 5.5 ELSE 0 END)+(CASE WHEN ПРЕДИКАТ2 THEN 3.5 ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldConvertMultiplicationWithOneTrueRule()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;
			const decimal Fac1 = 1.1m;
			const decimal Fac2 = 2.2m;

			var result = AggregateRuleResult.Build(RuleType, RuleResult.BuildTrue(RuleType, Fac2), RuleResult.BuildTrue(RuleType, Fac1));

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Fac1 * Fac2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "2.42");
		}

		[TestMethod]
		public void ShouldConvertMultiplicationWithTwoConvertibleRule()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;
			const decimal Fac1 = 1.1m;
			const decimal Fac2 = 2.2m;
			var ruleResults = new[]
				                  {
					                  RuleResult.BuildTrue(RuleType, Fac2),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), 5.5m),
					                  RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), 3.5m),
					                  RuleResult.BuildTrue(RuleType, Fac1)
				                  };

			var result = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, Fac1 * Fac2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "2.42*(CASE WHEN ПРЕДИКАТ1 THEN 5.5 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ2 THEN 3.5 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldAggregateWithCondResults()
		{
			const RuleTypes RuleType = RuleTypes.Addition;

			const decimal RuleFactor = 56.7m;
			const decimal CondFactor1 = 0.7834m;
			const decimal ConfFactor2 = 0.777m;

			var condRuleResults = new[]
				                      {
					                      ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ. ПРЕДИКАТ"), CondFactor1),
										  ConditionalResult.BuildTrue(ConfFactor2)
				                      };

			var result = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), RuleFactor, condRuleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРЕДИКАТ THEN CASE WHEN УСЛ. ПРЕДИКАТ THEN 0.7834 ELSE 0.777 END ELSE 0 END");

			const decimal Fac1 = 1.1m;
			var ruleResults = new[] { result, RuleResult.BuildTrue(RuleType, Fac1) };

			var aggregateRuleResult = AggregateRuleResult.Build(RuleType, ruleResults);

			Assert.AreEqual(aggregateRuleResult.Code, EvaluateResultCode.ConvertibleToSQL);

			Assert.AreEqual(TestHelper2.Convert(aggregateRuleResult), "1.1+(CASE WHEN ПРЕДИКАТ THEN CASE WHEN УСЛ. ПРЕДИКАТ THEN 0.7834 ELSE 0.777 END ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldAggregateTrueRuleResultAndTwoTrueAggregateRuleResult()
		{
			const RuleTypes RuleType = RuleTypes.Addition;

			var ruleResult = RuleResult.BuildTrue(RuleType, 5.5m);
			var aggreg1 = AggregateRuleResult.Build(RuleType, 6.6m);
			var aggreg2 = AggregateRuleResult.Build(RuleType, 1.1m);

			var result = AggregateRuleResult.Build(RuleType, ruleResult, aggreg1, aggreg2);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, 5.5m + 6.6m + 1.1m);
			Assert.AreEqual(TestHelper2.Convert(result), "13.2");
		}

		[TestMethod]
		public void ShouldAggregateTrueRuleResultAndOneTrueAggregateRuleResultAndOneConvertableAggregateRuleResult()
		{
			const RuleTypes RuleType = RuleTypes.Addition;

			var ruleResult = RuleResult.BuildTrue(RuleType, 5.5m);
			var aggreg1 = AggregateRuleResult.Build(RuleType, 6.6m);

			var aruleResult = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), 1.1m);

			var aggreg2 = AggregateRuleResult.Build(RuleType, aruleResult);

			Assert.AreEqual(aggreg2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(aggreg2), "(CASE WHEN ПРЕДИКАТ THEN 1.1 ELSE 0 END)");

			var result = AggregateRuleResult.Build(RuleType, ruleResult, aggreg1, aggreg2);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, 5.5m + 6.6m);
			Assert.AreEqual(TestHelper2.Convert(result), "12.1+(CASE WHEN ПРЕДИКАТ THEN 1.1 ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldAggregateTrueButNotAvaibleFactorRuleResultAndOneTrueAggregateRuleResultAndOneConvertableAggregateRuleResult()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var condResult = ConditionalResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), 2.2m);
			var ruleResult = RuleResult.BuildTrue(RuleType, 5.5m, condResult);
			Assert.AreEqual(ruleResult.Code, EvaluateResultCode.True);
			Assert.AreEqual(TestHelper2.Convert(ruleResult), "CASE WHEN ПРЕДИКАТ1 THEN 2.2 ELSE 5.5 END");

			var aggreg1 = AggregateRuleResult.Build(RuleType, 6.6m);

			var aruleResult = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), 1.1m);
			var aggreg2 = AggregateRuleResult.Build(RuleType, aruleResult);

			Assert.AreEqual(aggreg2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(aggreg2), "(CASE WHEN ПРЕДИКАТ2 THEN 1.1 ELSE 1 END)");

			var aggreg3 = AggregateRuleResult.Build(RuleType, 1.1m);

			var result = AggregateRuleResult.Build(RuleType, ruleResult, aggreg1, aggreg2, aggreg3);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, 7.26m);
			Assert.AreEqual(TestHelper2.Convert(result), "7.26*(CASE WHEN ПРЕДИКАТ1 THEN 2.2 ELSE 5.5 END)*(CASE WHEN ПРЕДИКАТ2 THEN 1.1 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldAggregateAggregateRuleResults()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = RuleResult.BuildTrue(RuleType, 1.1m);
			var rule2 = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), 2.2m);
			var rule3 = RuleResult.BuildTrue(RuleType, 3.3m);
			var rule4 = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ4"), 4.4m);
			
			var aggreg1 = AggregateRuleResult.Build(RuleType, rule1, rule2);
			var aggreg2 = AggregateRuleResult.Build(RuleType, rule3, rule4);

			var result = AggregateRuleResult.Build(RuleType, aggreg1, aggreg2);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, 3.63m);
			Assert.AreEqual(TestHelper2.Convert(result), "3.63*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END)");
		}
	}
}