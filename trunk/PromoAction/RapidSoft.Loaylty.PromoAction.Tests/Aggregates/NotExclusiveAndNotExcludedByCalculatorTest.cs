namespace RapidSoft.Loaylty.PromoAction.Tests.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class NotExclusiveAndNotExcludedByCalculatorTest
	{
		// NOTE: Фильтр не исключающих, не исключаемых правил.
		// Используется когда найдено как минимум одно исключающее правило.
		private Func<Rule, bool> GetFilter2()
		{
			return r => !r.IsExclusive && r.IsNotExcludedBy;
		}

		[TestMethod]
		public void ShouldCalculateOneTrueRule()
		{
			const decimal Coef = 5.5m;
			const RuleTypes RuleType = RuleTypes.Addition;

			var rule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var otherRule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule, RuleResult.BuildTrue(RuleType, Coef) },
							{ otherRule, RuleResult.BuildTrue(RuleType, Coef) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveAndNotExcludedByCalculator(RuleType, settings);
			var rules = new[] { rule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter2(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef);
			Assert.AreEqual(TestHelper2.Convert(result), "5.5");
		}

		[TestMethod]
		public void ShouldCalculateTwoTrueAdditionRules()
		{
			const decimal Coef = 5.5m;
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 7.2m;
			const RuleTypes RuleType = RuleTypes.Addition;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var otherRule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) },
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) },
							{ otherRule, RuleResult.BuildTrue(RuleType, Coef) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveAndNotExcludedByCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, otherRule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter2(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef1 + Coef2);
			Assert.AreEqual(TestHelper2.Convert(result), "12.7");
		}

		[TestMethod]
		public void ShouldCalculateTwoTrueMultiplicationRules()
		{
			const decimal Coef = 5.5m;
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 7.2m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var otherRule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) },
							{ otherRule, RuleResult.BuildTrue(RuleType, Coef) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveAndNotExcludedByCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, otherRule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter2(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef1 * Coef2);
			Assert.AreEqual(TestHelper2.Convert(result), "39.6");
		}

		[TestMethod]
		public void ShouldRenderOneConvertRule()
		{
			const decimal Coef = 5.5m;
			const RuleTypes RuleType = RuleTypes.Addition;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var otherRule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО"), Coef) },
							{ otherRule, RuleResult.BuildTrue(RuleType, Coef) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveAndNotExcludedByCalculator(RuleType, settings);
			var rules = new[] { rule, otherRule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter2(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result), "(CASE WHEN ПРАВИЛО THEN 5.5 ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldRenderTwoConvertRule()
		{
			const decimal Coef = 5.5m;
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 7.2m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var otherRule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{
								rule1,
								RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО1"), Coef1)
							},
							{
								rule2,
								RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО2"), Coef2)
							},
							{ otherRule, RuleResult.BuildTrue(RuleType, Coef) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveAndNotExcludedByCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, otherRule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter2(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result), "(CASE WHEN ПРАВИЛО1 THEN 5.5 ELSE 1 END)*(CASE WHEN ПРАВИЛО2 THEN 7.2 ELSE 1 END)");
		}
	}
}
