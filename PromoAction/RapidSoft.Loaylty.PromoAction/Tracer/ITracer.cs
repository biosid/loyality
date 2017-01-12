namespace RapidSoft.Loaylty.PromoAction.Tracer
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    public interface ITracer
    {
        void Trace(RuleTypes ruleType, string comment = null);

        void Trace(Rule rule, RuleResult ruleResult, string comment = null);

        void Trace(Rule rule, EvaluateResult evalResult, string comment = null);

        void Trace(RuleTypes ruleType, AggregateRuleResult ruleResult, string comment);

        string[] GetMessages();
    }
}