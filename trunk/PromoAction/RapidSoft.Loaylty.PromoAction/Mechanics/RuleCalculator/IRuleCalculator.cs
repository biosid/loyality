namespace RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
    /// Интерфейс логики вычисления правила.
    /// </summary>
    public interface IRuleCalculator : IPredicateCalculator
    {
        /// <summary>
        /// Вычисляет правило.
        /// </summary>
        /// <param name="rule">
        /// Вычисляемое правило.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        RuleResult CalculateRule(Rule rule);
    }
}