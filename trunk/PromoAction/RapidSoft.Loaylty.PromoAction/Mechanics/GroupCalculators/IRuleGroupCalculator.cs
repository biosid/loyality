namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Интерфейс вычислятеля группы правил.
    /// </summary>
    public interface IRuleGroupCalculator
    {
        /// <summary>
        /// Вычисляет группу правил выбирая необходимые правила.
        /// </summary>
        /// <param name="rules">
        /// Коллекция правил домена.
        /// </param>
        /// <returns>
        /// Результат вычисления группы правил.
        /// </returns>
        RuleGroupResult Calculate(IEnumerable<Rule> rules);
    }
}