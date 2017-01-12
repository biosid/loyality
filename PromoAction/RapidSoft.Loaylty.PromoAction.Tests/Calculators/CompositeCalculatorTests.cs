namespace RapidSoft.Loaylty.PromoAction.Tests.Calculators
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class CompositeCalculatorTests
	{
        private readonly ILog log = LogManager.GetLogger(typeof(CompositeCalculatorTests));

        [TestMethod]
		public void ShouldReturnCalculator()
		{
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: new MockRuleCalculator(null));

			var calculator = CalculatorFactory.GetCalculator(settings);

			Assert.IsNotNull(calculator);
		}

		[TestMethod]
		public void ShouldApplyBase()
		{
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: new MockRuleCalculator(null));

			var calculator = CalculatorFactory.GetCalculator(settings);
			var s = Stopwatch.StartNew();
			calculator.Calculate(new Rule[0]);
			s.Stop();
			log.Info("Time = " + s.ElapsedMilliseconds + " мс.");
		}

		[TestMethod]
		public void ShouldCalculateRuleDomain()
		{
			const decimal Fac1 = 1.1m; // БМК
			const decimal Fac2 = 2.2m; // БАК
			const decimal Fac3 = 3.3m;
			const decimal Fac4 = 4.4m;

			var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication };
			var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition };
			var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication };
			var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition };

			var ruleCalculator =
				new MockRuleCalculator(
					new Dictionary<Rule, RuleResult>
						{
							{ rule1, RuleResult.BuildTrue(RuleTypes.BaseMultiplication, Fac1) }, 
							{ rule2, RuleResult.BuildTrue(RuleTypes.BaseAddition, Fac2) },
							{ rule3, RuleResult.BuildTrue(RuleTypes.Multiplication, Fac3) },
							{ rule4, RuleResult.BuildTrue(RuleTypes.Addition, Fac4) },
						});
            var settings = new EvaluationSettings(TestHelper2.MockTracer, ruleCalculator: ruleCalculator);

			var calculator = CalculatorFactory.GetCalculator(settings);

			var rules = new List<Rule> { rule1, rule2, rule3, rule4 };

			var results = calculator.Calculate(rules);

			Assert.IsNotNull(results.SingleOrDefault(x => x.RuleType == RuleTypes.BaseMultiplication && x.Factor == Fac1));
			Assert.IsNotNull(results.SingleOrDefault(x => x.RuleType == RuleTypes.BaseAddition && x.Factor == Fac2));
			Assert.IsNotNull(results.SingleOrDefault(x => x.RuleType == RuleTypes.Multiplication && x.Factor == Fac3));
			Assert.IsNotNull(results.SingleOrDefault(x => x.RuleType == RuleTypes.Addition && x.Factor == Fac4));
		}
	}
}
