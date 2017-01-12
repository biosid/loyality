namespace RapidSoft.Loaylty.PromoAction.Tests.GroupCalculators
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class BaseRulesCalculatorTests
	{
		[TestMethod]
		public void ShouldCalculate()
		{
			const decimal Coef = 1.1m;
			const RuleTypes RuleType = RuleTypes.BaseAddition;

			var rule = new Rule { Factor = Coef, Type = RuleType };

			IRuleCalculator selector = new MockRuleCalculator(new Dictionary<Rule, RuleResult> { { rule, RuleResult.BuildTrue(RuleType, Coef) } });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);
			var rules = new[] { rule };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, Coef);
			Assert.AreEqual(TestHelper2.Convert(result), "1.1");
		}

		[TestMethod]
		public void ShouldConvertOneConvertible()
		{
			const int Coef = 5;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			var defaultFactor = RuleType.GetDefaultFactor();
			
			var rule = new Rule { Factor = Coef, Type = RuleType };

			IRuleCalculator selector =
				new MockRuleCalculator(new Dictionary<Rule, RuleResult>
					                       {
						                       { rule, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРАВИЛО"), Coef) }
					                       });
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);
			var rules = new[] { rule };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРАВИЛО THEN 5 ELSE 0 END");
		}

		[TestMethod]
		public void ShouldConvertManyConvertibleWithCondResults()
		{
			const decimal Coef1 = 5.1m;
			const decimal Coef2 = 5.2m;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, Priority = 100 };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, Priority = 99 };

			var condRuleResult = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ"), 1.1m);

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) },
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2, condRuleResult) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);

			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN 5.1 " +
					"WHEN ПРЕДИКАТ2 THEN " +
						"CASE " +
							"WHEN УСЛ.ПРЕДИКАТ " +
							"THEN 1.1 " +
							"ELSE 5.2 " +
						"END " +
					"ELSE 0 " +
				"END");
		}

		[TestMethod]
		public void ShouldConvertManyConvertibleWithTwoCondResults()
		{
			const decimal Coef1 = 5.1m;
			const decimal Coef2 = 5.2m;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, Priority = 100 };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, Priority = 99 };

			var condRuleResult1 = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ1"), 1.1m);
			var condRuleResult2 = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ2"), 2.2m);

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) },
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2, condRuleResult1, condRuleResult2) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);

			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN 5.1 " +
					"WHEN ПРЕДИКАТ2 THEN " +
						"CASE " +
							"WHEN УСЛ.ПРЕДИКАТ1 THEN 1.1 " +
							"WHEN УСЛ.ПРЕДИКАТ2 THEN 2.2 " +
							"ELSE 5.2 " +
						"END " +
					"ELSE 0 " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWithPriority()
		{
			const decimal Coef = 5.5m;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			const int Priority = 10;

			var rule1 = new Rule { Factor = Coef - 10, Type = RuleType, Priority = Priority - 10 };
			var rule2 = new Rule { Factor = Coef - 5, Type = RuleType, Priority = Priority - 5 };
			var rule3 = new Rule { Factor = Coef, Type = RuleType, Priority = Priority };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef - 10) },
							{ rule2, RuleResult.BuildTrue(RuleType, Coef - 5) },
							{ rule3, RuleResult.BuildTrue(RuleType, Coef) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);

			var result = calculator.Calculate(new[] { rule1, rule2, rule3 });

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, Coef);
			Assert.AreEqual(TestHelper2.Convert(result), "5.5");
		}

		[TestMethod]
		public void ShouldCalculateWithPriorityAndCondFactors()
		{
			const decimal Coef = 5.5m;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			const int Priority = 10;

			var rule1 = new Rule { Factor = Coef - 10, Type = RuleType, Priority = Priority - 10 };
			var rule2 = new Rule { Factor = Coef - 5, Type = RuleType, Priority = Priority - 5 };
			var rule3 = new Rule { Factor = Coef, Type = RuleType, Priority = Priority };

			var condRule1 = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ1"), 3.3m);
			var condRule2 = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ2"), 3.4m);
			var condRule3 = ConditionalResult.BuildTrue(3.5m);

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef - 10) },
							{ rule2, RuleResult.BuildTrue(RuleType, Coef - 5) },
							{ rule3, RuleResult.BuildTrue(RuleType, Coef, new[] { condRule1, condRule2, condRule3 }) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);

			var result = calculator.Calculate(new[] { rule1, rule2, rule3 });

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, RuleType.GetDefaultFactor());
			Assert.AreEqual(
				TestHelper2.Convert(result), 
				"CASE " + 
					"WHEN УСЛ.ПРЕДИКАТ1 THEN 3.3 " +
					"WHEN УСЛ.ПРЕДИКАТ2 THEN 3.4 " +
					"ELSE 3.5 " +
				"END");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidPriorityException))]
		public void ShouldNotCalculateWithDoublePriority()
		{
			const int Coef = 5;
			const RuleTypes RuleType = RuleTypes.BaseAddition;
			const int Priority = 10;

			var rule1 = new Rule { Factor = Coef - 10, Type = RuleType, Priority = Priority };
			var rule2 = new Rule { Factor = Coef - 5, Type = RuleType, Priority = Priority - 5 };
			var rule3 = new Rule { Factor = Coef, Type = RuleType, Priority = Priority };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef - 10) },
							{ rule2, RuleResult.BuildTrue(RuleType, Coef - 5) },
							{ rule3, RuleResult.BuildTrue(RuleType, Coef) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new BaseRulesCalculator(RuleType, settings);

			calculator.Calculate(new[] { rule1, rule2, rule3 });

			Assert.Fail("Должен быть Exception");
		}
	}
}
