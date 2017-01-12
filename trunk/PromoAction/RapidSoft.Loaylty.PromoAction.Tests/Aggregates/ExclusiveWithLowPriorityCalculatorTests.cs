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
	public class ExclusiveWithLowPriorityCalculatorTests
	{
		// NOTE: Фильтр исключающих, не исключаемых правил с приоритетом ниже заданного.
		// Используется когда найдено как минимум одно исключающее правило которое конвертируется в SQL.
		private Func<Rule, bool> GetFilter1(int priority)
		{
			return r => r.IsExclusive && r.IsNotExcludedBy && r.Priority < priority;
		}

		[TestMethod]
		public void ShouldCalculateTwoTrueAdditionRules()
		{
			const int LastPriority = 6;
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 5.5m;
			const decimal Coef = 3.3m;
			const RuleTypes RuleType = RuleTypes.Addition;

			var rule1 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority + 1 };
			var rule2 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority };
			var rule3 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 1 };
			var rule4 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 2 };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult>
					                       {
						                       { rule1, RuleResult.BuildTrue(RuleType, Coef) },
						                       { rule2, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule3, RuleResult.BuildTrue(RuleType, Coef1) },
											   { rule4, RuleResult.BuildTrue(RuleType, Coef2) }
					                       });
			var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3, rule4 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter1(LastPriority), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.AggregateFactor, Coef1 + Coef2);
			Assert.AreEqual(TestHelper2.Convert(result), "11");
		}

		[TestMethod]
		public void ShouldRenderOneConvertRule()
		{
			const int LastPriority = 6;
			const decimal Coef1 = 5.5m;
			const decimal Coef = 3.3m;
			const RuleTypes RuleType = RuleTypes.Addition;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority + 1 };
			var rule2 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority };
			var rule3 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 1 };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult>
										   {
											   { rule1, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule2, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО"), Coef1) }
										   });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter1(LastPriority), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result), "(CASE WHEN ПРАВИЛО THEN 5.5 ELSE 0 END)");
		}

		[TestMethod]
		public void ShouldRenderTwoConvertRule()
		{
			const int LastPriority = 6;
			const decimal Coef1 = 5.4m;
			const decimal Coef2 = 5.5m;
			const decimal Coef = 3.3m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority + 1 };
			var rule2 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority };
			var rule3 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 1 };
			var rule4 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 2 };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult>
										   {
											   { rule1, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule2, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО1"), Coef1) },
											   { rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО2"), Coef2) }
										   });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3, rule4 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter1(LastPriority), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result), "(CASE WHEN ПРАВИЛО1 THEN 5.4 ELSE 1 END)*(CASE WHEN ПРАВИЛО2 THEN 5.5 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldRenderOneTrueAndOneConvertRule()
		{
			const int LastPriority = 6;
			const decimal Coef1 = 5.4m;
			const decimal Coef2 = 5.5m;
			const decimal Coef = 3.3m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority + 1 };
			var rule2 = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority };
			var rule3 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 1 };
			var rule4 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = LastPriority - 2 };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult>
										   {
											   { rule1, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule2, RuleResult.BuildTrue(RuleType, Coef) },
											   { rule3, RuleResult.BuildTrue(RuleType, Coef1) },
											   { rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО4"), Coef2) }
										   });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3, rule4 };

			var calculator = new AggregateCalculator(RuleType, this.GetFilter1(LastPriority), settings);

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.AggregateFactor, rule3.Factor);
			Assert.AreEqual(TestHelper2.Convert(result), "5.4*(CASE WHEN ПРАВИЛО4 THEN 5.5 ELSE 1 END)");
		}
	}
}