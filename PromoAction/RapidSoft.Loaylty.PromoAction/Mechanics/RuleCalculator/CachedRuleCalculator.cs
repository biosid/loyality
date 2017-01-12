namespace RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator
{
    using System.Collections.Generic;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Tracer;

    /// <summary>
    /// Кэширующий калькулятора правил. Эффективность использования данного калькулятора заметна при наличии более одного исключающего правила в группе
    /// </summary>
    public class CachedRuleCalculator : IRuleCalculator
    {
        /// <summary>
        /// Не кэширующий калькулятор.
        /// </summary>
        private readonly IRuleCalculator baseRuleCalculator;

        private readonly ITracer tracer;

        /// <summary>
        /// Кэш вычислений.
        /// </summary>
        private readonly Dictionary<long, RuleResult> cache = new Dictionary<long, RuleResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRuleCalculator"/> class.
        /// </summary>
        /// <param name="baseRuleCalculator">
        /// Не кэширующий калькулятор.
        /// </param>
        public CachedRuleCalculator(ITracer tracer, IRuleCalculator baseRuleCalculator)
        {
            tracer.ThrowIfNull("tracer");
            baseRuleCalculator.ThrowIfNull("baseRuleCalculator");

            this.baseRuleCalculator = baseRuleCalculator;
            this.tracer = tracer;
        }

        /// <summary>
        /// Вычисляет предикат.
        /// </summary>
        /// <param name="predicate">
        /// Вычислемый предикат.
        /// </param>
        /// <returns>
        /// Результат вычисления предиката.
        /// </returns>
        public EvaluateResult EvaluatePredicate(filter predicate)
        {
            return this.baseRuleCalculator.EvaluatePredicate(predicate);
        }

        /// <summary>
        /// Вычисляет правило, проверяя не вычислялось ли правило ранее по кэшу.
        /// </summary>
        /// <param name="rule">
        /// Вычисляемое правило.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        public RuleResult CalculateRule(Rule rule)
        {
            if (this.cache.ContainsKey(rule.Id))
            {
                var retVal = this.cache[rule.Id];

                this.tracer.Trace(rule, retVal, "Результат получен из кэша");

                return retVal;
            }

            var result = this.baseRuleCalculator.CalculateRule(rule);

            this.cache.Add(rule.Id, result);

            return result;
        }
    }
}