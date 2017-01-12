namespace RapidSoft.Loaylty.PromoAction.Tests.Mocks
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tracer;

    public class MockTracer : ITracer
    {
        public void Trace(RuleTypes ruleType, string comment = null)
        {
        }

        public void Trace(Rule rule, RuleResult ruleResult, string comment = null)
        {
        }

        public void Trace(Rule rule, EvaluateResult evalResult, string comment = null)
        {
        }

        public void Trace(RuleTypes ruleType, AggregateRuleResult ruleResult, string comment)
        {
        }

        public string[] GetMessages()
        {
            return new string[0];
        }
    }
}