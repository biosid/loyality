namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    /// <summary>
    /// Вычислятель базовых правил.
    /// </summary>
    public class BaseRulesCalculator : IRuleGroupCalculator
    {
        /// <summary>
        /// Настройки вычисления.
        /// </summary>
        private readonly EvaluationSettings settings;

        /// <summary>
        /// Тип правил вычисляемый калькулятором.
        /// </summary>
        private readonly RuleTypes ruleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRulesCalculator"/> class.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        /// <param name="settings">
        /// Настройки вычисления.
        /// </param>
        public BaseRulesCalculator(RuleTypes ruleType, EvaluationSettings settings)
        {
            if (ruleType == RuleTypes.Addition || ruleType == RuleTypes.Multiplication)
            {
                throw new InvalidRuleGroupTypeException("Калькулятор базовых правил может вычислять только базовые правила");
            }

            this.settings = settings;
            this.ruleType = ruleType;
        }

        /// <summary>
        /// Вычисляет группу правил выбирая необходимые правила.
        /// </summary>
        /// <param name="rules">
        /// Коллекция правил домена.
        /// </param>
        /// <returns>
        /// Результат вычисления группы правил.
        /// </returns>
        public RuleGroupResult Calculate(IEnumerable<Rule> rules)
        {
            rules.ThrowIfNull("rules");

            var rulesByType = rules.Where(x => x.Type == this.ruleType).ToArray();

            var groupedRules = rulesByType.GroupBy(r => r.Priority).OrderByDescending(x => x.Key);

            var ruleResults = groupedRules.Select(this.CalculateRuleGroup).TakeToFirstTrue().ToArray();

            switch (ruleResults.Length)
            {
                case 0:
                    {
                        return RuleGroupResult.BuildFalse(this.ruleType);
                    }

                case 1:
                    {
                        var ruleResult = ruleResults.First();

                        switch (ruleResult.Code)
                        {
                            case EvaluateResultCode.False:
                                {
                                    return RuleGroupResult.BuildFalse(this.ruleType);
                                }

                            case EvaluateResultCode.True:
                                {
                                    if (ruleResult.IsFactorAvaible)
                                    {
                                        return RuleGroupResult.BuildTrue(this.ruleType, ruleResult.Factor);
                                    }

                                    return RuleGroupResult.BuildConvertible(this.ruleType, ruleResult.Converter);
                                }

                            case EvaluateResultCode.ConvertibleToSQL:
                                {
                                    return RuleGroupResult.BuildConvertible(this.ruleType, ruleResult.Converter);
                                }

                            default:
                                {
                                    throw new NotSupportedException();
                                }
                        }
                    }

                default:
                    {
                        return RuleGroupResult.BuildConvertible(this.ruleType, ruleResults);
                    }
            }
        }

        /// <summary>
        /// Проверяет наличие в групе по приоритету только одного правила и вычисляет результат правила.
        /// </summary>
        /// <param name="ruleGroup">
        /// Группа правил по приоритету.
        /// </param>
        /// <returns>
        /// Результат вычисления единственного правила в группе.
        /// </returns>
        private RuleResult CalculateRuleGroup(IGrouping<int, Rule> ruleGroup)
        {
            ruleGroup.ThrowIfNull("ruleGroup");

            if (ruleGroup.Count() > 1)
            {
                throw new InvalidPriorityException("Должно быть только одно правило по приоритету");
            }

            var ruleFromGroup = ruleGroup.First();

            var retVal = this.settings.RuleCalculator.CalculateRule(ruleFromGroup);

            this.settings.Tracer.Trace(ruleFromGroup, retVal);

            return retVal;
        }
    }
}