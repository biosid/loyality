namespace RapidSoft.Loaylty.PromoAction.Tests.GroupCalculators
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class ExclusiveCalculatorTests
	{
		[TestMethod]
		public void ShouldCalculateWhenNotExitsExclusiveAndOthresTrue()
		{
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }
						});

            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2 };
			
			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.True);
			Assert.AreEqual(result2.Factor, Coef1 * Coef2);
			Assert.AreEqual(TestHelper2.Convert(result2), "36.3");
		}

		[TestMethod]
		public void ShouldCalculateWhenNotExitsExclusiveAndOthresTrueButOneTrueIsNotAvaible()
		{
			const decimal Coef1 = 5.5m;
			const decimal Coef2 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			var condRuleResult = ConditionalResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), 1.1m);

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) },
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2, new[] { condRuleResult }) }
						});

            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, RuleType.GetDefaultFactor());
			Assert.AreEqual(TestHelper2.Convert(result2), "5.5*(CASE WHEN ПРЕДИКАТ THEN 1.1 ELSE 6.6 END)");
		}

		[TestMethod]
		public void ShouldCalculateWhenNotExitsExclusiveAndOthresConvertable()
		{
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }
						});

            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, RuleType.GetDefaultFactor());
			Assert.AreEqual(TestHelper2.Convert(result2), "(CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldCalculateWhenExistsFalseExclusive()
		{
			const decimal Coef = 3.3m;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule = new Rule { Factor = Coef, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false };
			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }, 
							{ rule, RuleResult.BuildFalse(RuleType) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);
			var rules = new[] { rule1, rule2, rule };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.True);
			Assert.AreEqual(result2.Factor, Coef1 * Coef2);
			Assert.AreEqual(TestHelper2.Convert(result2), "2.42");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneTrueExclusiveAndOneTrueExclusiveWithLowPriorityAndOneTrueNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var ignoredRule2 = new Rule { Id = 10, Factor = 10, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var ignoredRule1 = new Rule { Id = 11, Factor = 100, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) }, 
							{ ignoredRule2, RuleResult.BuildTrue(RuleType, 10) }, 
							{ ignoredRule1, RuleResult.BuildTrue(RuleType, 100) }, 
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, ignoredRule1, rule2, rule3, ignoredRule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.True);
			Assert.AreEqual(result2.Factor, Coef1 * Coef2 * Coef3);
			Assert.AreEqual(TestHelper2.Convert(result2), "7.986");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneTrueExclusiveAndOneTrueExclusiveWithLowPriorityAndOneConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 3.3m;
			const decimal Coef2 = 5.5m;
			const decimal Coef3 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var ignoredRule2 = new Rule { Id = 10, Factor = 10, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var ignoredRule1 = new Rule { Id = 11, Factor = 100, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }, 
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), Coef3) },
							{ ignoredRule2, RuleResult.BuildTrue(RuleType, 10) },
							{ ignoredRule1, RuleResult.BuildTrue(RuleType, 100) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, ignoredRule1, rule2, rule3, ignoredRule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, 1);
			Assert.AreEqual(TestHelper2.Convert(result2), "18.15*(CASE WHEN ПРЕДИКАТ THEN 6.6 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneTrueExclusiveAndOneConvertibleExclusiveWithLowPriorityAndOneTrueNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 3.3m;
			const decimal Coef2 = 5.5m;
			const decimal Coef3 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var ignoredRule1 = new Rule { Id = 10, Factor = 100, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var ignoredRule2 = new Rule { Id = 11, Factor = 10, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ ignoredRule1, RuleResult.BuildTrue(RuleType, 100) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) },
							{ ignoredRule2, RuleResult.BuildTrue(RuleType, 10) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, ignoredRule1, rule2, rule3, ignoredRule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);
			Assert.AreEqual(TestHelper2.Convert(result2), "21.78*(CASE WHEN ПРЕДИКАТ THEN 5.5 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneTrueExclusiveAndOneConvertibleExclusiveWithLowPriorityAndOneConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 3.3m;
			const decimal Coef2 = 5.5m;
			const decimal Coef3 = 6.6m;
			const RuleTypes RuleType = RuleTypes.Multiplication;

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var ignoredRule1 = new Rule { Id = 10, Factor = 100, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var ignoredRule2 = new Rule { Id = 30, Factor = 10, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleType, Coef1) }, 
							{ ignoredRule1, RuleResult.BuildTrue(RuleType, 100) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef3) },
							{ ignoredRule2, RuleResult.BuildTrue(RuleType, 10) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);
			var rules = new[] { rule1, ignoredRule1, rule2, rule3, ignoredRule2 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, 1);
			Assert.AreEqual(TestHelper2.Convert(result2), "3.3*(CASE WHEN ПРЕДИКАТ2 THEN 5.5 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 6.6 ELSE 1 END)");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneConvertibleExclusiveAndOneTrueNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);

			var elseFactor = (Coef2 * Coef3).ToString(CultureInfo.InvariantCulture);
			Assert.AreEqual(TestHelper2.Convert(result2), "CASE WHEN ПРЕДИКАТ THEN 2.2*(CASE WHEN ПРЕДИКАТ THEN 1.1 ELSE 1 END) ELSE " + elseFactor + " END");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneConvertibleExclusiveAndOneConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 3.3m;
			const decimal Coef2 = 6.6m;
			const decimal Coef3 = 7.7m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) }
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);
			var rules = new[] { rule1, rule2, rule3 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);

			/* TODO: Можно определить что правило не содержит условных коэффициентов и тогда сделать так: 
				CASE WHEN ПРЕДИКАТ1 THEN 3.3*(CASE WHEN ПРЕДИКАТ2 THEN 6.6 ELSE 1 END) ELSE 7.7 END
			 */
			Assert.AreEqual(
				TestHelper2.Convert(result2),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 3.3 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ2 THEN 6.6 ELSE 1 END) " +
					"ELSE 7.7*(CASE WHEN ПРЕДИКАТ2 THEN 6.6 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneConvertibleExclusiveAndTwoConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule4 = new Rule { Id = 3, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };
			var rule3 = new Rule { Id = 4, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule4, RuleResult.BuildTrue(RuleType, Coef4) },
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef3) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule4, rule3 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result2), 
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
					"ELSE 4.4*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneConvertibleExclusiveWithCondResultsAndTwoConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			const decimal Coef5 = 0.005m;

			var condResult = ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ. ПРЕДИКАТ"), Coef5);

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1, condResult) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef3) },
							{ rule4, RuleResult.BuildTrue(RuleType, Coef4) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule4, rule3 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result2),
				"CASE " +
					"WHEN ПРЕДИКАТ1 " +
						"THEN (CASE WHEN ПРЕДИКАТ1 THEN CASE WHEN УСЛ. ПРЕДИКАТ THEN 0.005 ELSE 1.1 END ELSE 1 END)*" +
							"(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
					"ELSE 4.4*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenManyConvertibleExclusiveAndTwoConvertibleNotExcludedBy()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef3) },
							{ rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ4"), Coef4) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3, rule4 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result2),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
					"WHEN ПРЕДИКАТ2 THEN (CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END) " +
					"ELSE (CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenManyConvertibleExclusiveAndOthersFalse()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Factor = Coef3, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule4 = new Rule { Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildFalse(RuleType) },
							{ rule4, RuleResult.BuildFalse(RuleType) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var rules = new[] { rule1, rule2, rule3, rule4 };

			var calculator2 = new ExclusiveCalculator(RuleType, settings);
			var result2 = calculator2.Calculate(rules);

			Assert.AreEqual(result2.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result2.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result2),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END) " + 
					"WHEN ПРЕДИКАТ2 THEN (CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END) " + 
					"ELSE 1 " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenManyConvertibleExclusiveAndNotFound()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new ExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " + 
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END) " +
					"WHEN ПРЕДИКАТ2 THEN (CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END) " + 
					"ELSE 1 " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenOneConvertibleAndOneTrueExclusiveAndConvertableExclusiveNotExcludedByLowPriorityAndOthersConvertable()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const decimal Coef5 = 5.5m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var ignoredRule5 = new Rule { Id = 5, Factor = Coef5, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildTrue(RuleType, Coef2) }, 
							{ rule3, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ3"), Coef3) },
							{ rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ4"), Coef4) },
							{ ignoredRule5, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ5"), Coef5) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new ExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, rule3, rule4, ignoredRule5 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN (CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " + 
					"ELSE 2.2*(CASE WHEN ПРЕДИКАТ3 THEN 3.3 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenTwoConvertibleAndOneTrueExclusiveAndConvertableExclusiveNotExcludedByLowPriorityNotFoundAndOthersConvertable()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const decimal Coef5 = 5.5m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule5 = new Rule { Id = 5, Factor = Coef5, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) },
							{ rule4, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ4"), Coef4) },
							{ rule5, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ5"), Coef5) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new ExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, rule3, rule4, rule5 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN 3.3*(CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " +
					"WHEN ПРЕДИКАТ2 THEN 3.3*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END)*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " +
					"ELSE 3.3*(CASE WHEN ПРЕДИКАТ4 THEN 4.4 ELSE 1 END) " +
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenTwoConvertibleAndOneTrueExclusiveAndConvertableExclusiveNotExcludedByLowPriorityNotFoundAndOthersFalse()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const decimal Coef5 = 5.5m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule5 = new Rule { Id = 5, Factor = Coef5, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) },
							{ rule4, RuleResult.BuildFalse(RuleType) },
							{ rule5, RuleResult.BuildFalse(RuleType) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new ExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, rule3, rule4, rule5 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);
			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " + 
					"WHEN ПРЕДИКАТ1 THEN 3.3*(CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END) " + 
					"WHEN ПРЕДИКАТ2 THEN 3.3*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END) " + 
					"ELSE 3.3 " + 
				"END");
		}

		[TestMethod]
		public void ShouldCalculateWhenTwoConvertibleAndOneTrueExclusiveAndConvertableExclusiveNotExcludedByLowPriorityNotFoundAndNotExcludedByTrue()
		{
			const int Priority = 5;
			const decimal Coef1 = 1.1m;
			const decimal Coef2 = 2.2m;
			const decimal Coef3 = 3.3m;
			const decimal Coef4 = 4.4m;
			const decimal Coef5 = 5.5m;
			const RuleTypes RuleType = RuleTypes.Multiplication;
			var defaultFactor = RuleType.GetDefaultFactor();

			var rule1 = new Rule { Id = 1, Factor = Coef1, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority };
			var rule2 = new Rule { Id = 2, Factor = Coef2, Type = RuleType, IsExclusive = true, IsNotExcludedBy = false, Priority = Priority - 1 };
			var rule3 = new Rule { Id = 3, Factor = Coef3, Type = RuleType, IsExclusive = true, IsNotExcludedBy = true, Priority = Priority - 2 };
			var rule4 = new Rule { Id = 4, Factor = Coef4, Type = RuleType, IsExclusive = false, IsNotExcludedBy = true };
			var rule5 = new Rule { Id = 5, Factor = Coef5, Type = RuleType, IsExclusive = false, IsNotExcludedBy = false };

			IRuleCalculator selector =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ1"), Coef1) }, 
							{ rule2, RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ2"), Coef2) }, 
							{ rule3, RuleResult.BuildTrue(RuleType, Coef3) },
							{ rule4, RuleResult.BuildTrue(RuleType, Coef4) },
							{ rule5, RuleResult.BuildFalse(RuleType) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: selector);

			var calculator = new ExclusiveCalculator(RuleType, settings);
			var rules = new[] { rule1, rule2, rule3, rule4, rule5 };

			var result = calculator.Calculate(rules);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, defaultFactor);

			// 14.52 = 3.3 * 4.4
			Assert.AreEqual(
				TestHelper2.Convert(result),
				"CASE " +
					"WHEN ПРЕДИКАТ1 THEN 14.52*(CASE WHEN ПРЕДИКАТ1 THEN 1.1 ELSE 1 END) " +
					"WHEN ПРЕДИКАТ2 THEN 14.52*(CASE WHEN ПРЕДИКАТ2 THEN 2.2 ELSE 1 END) " +
					"ELSE 14.52 " +
				"END"); 
		}
	}
}
