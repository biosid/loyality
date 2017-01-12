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
	public class NotExclusiveCalculatorTests
	{
		// NOTE: Фильтр не исключающих правил, без учета признака "не исключаемое".
		// Используется когда не найдено ни одного исключающего правила
		private Func<Rule, bool> GetFilter3()
		{
			return r => !r.IsExclusive;
		}

		[TestMethod]
		public void ShouldCalculate()
		{
			const int Coef = 5;
			const RuleTypes RuleType = RuleTypes.Addition;

			var rule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = false };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult> { { rule, RuleResult.BuildTrue(RuleType, Coef) } });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter3(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "5");
		}

		[TestMethod]
		public void ShouldCalculateTwoTrueRules()
		{
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var ignoredRule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false };
			var ignoredRule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, ignoredRule1, ignoredRule2 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter3(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef1 * Coef2);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "36.3");
		}

		[TestMethod]
		public void ShouldCalculateTwoConvertableRules()
		{
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var ignoredRule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false };
			var ignoredRule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, ignoredRule1, ignoredRule2 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter3(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "(CASE WHEN ПРЕДИКАТ1 THEN 5.5 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ2 THEN 6.6 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldCalculateTwoTrueAndTwoConvertableRules()
		{
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 6.6m;
			const decimal Coef3 = 1.1m;
			const decimal Coef4 = 2.2m;
			const RuleTypes RuleType = RuleTypes.Addition;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var rule3 = new Rule { Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule4 = new Rule { Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var ignoredRule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false };
			var ignoredRule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef2) },
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) },
							{ rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ4"), Coef4) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			// var calculator = new NotExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, rule3, rule4, ignoredRule1, ignoredRule2 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter3(), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, Coef1 + Coef3);
			Assert.AreEqual(TestHelper2.Convert(result.Converter), "6.6+(CASE WHEN ПРЕДИКАТ3 THEN 6.6 ELSE 0 END)+(CASE WHEN ПРЕДИКАТ4 THEN 2.2 ELSE 0 END)");
		}
	}
}