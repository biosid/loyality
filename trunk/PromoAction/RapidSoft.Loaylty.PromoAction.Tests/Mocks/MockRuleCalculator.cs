namespace RapidSoft.Loaylty.PromoAction.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	public class MockRuleCalculator : IRuleCalculator
	{
		private readonly IDictionary<Rule, RuleResult> ruleResults;

		public MockRuleCalculator(IDictionary<Rule, RuleResult> ruleResults)
		{
			this.ruleResults = ruleResults;
		}

		public RuleResult CalculateRule(Rule rule)
		{
			return this.ruleResults[rule];
		}

		public EvaluateResult EvaluatePredicate(filter predicate)
		{
			throw new NotSupportedException();
		}
	}
}
