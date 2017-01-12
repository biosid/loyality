namespace RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
    /// Реализация калькулятора правила <see cref="IRuleCalculator"/>.
    /// </summary>
    public class RuleCalculator : IRuleCalculator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RuleCalculator));

        /// <summary>
        /// Настройки вычисления.
        /// </summary>
        private readonly EvaluationSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleCalculator"/> class.
        /// </summary>
        /// <param name="settings">
        /// Настройки вычис.
        /// </param>
        public RuleCalculator(EvaluationSettings settings)
        {
            settings.ThrowIfNull("settings");

            this.settings = settings;
        }

        /// <summary>
        /// Вычисляет правило.
        /// </summary>
        /// <param name="rule">
        /// Вычисляемое правило.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        public RuleResult CalculateRule(Rule rule)
        {
            rule.ThrowIfNull("rule");

            log.DebugFormat("Вычисляем предикат правила: {0}", rule);
            var predicate = rule.GetDeserializedPredicate();

            var evalResult = this.EvaluatePredicate(predicate);

            this.settings.Tracer.Trace(rule, evalResult);
            
            switch (evalResult.Code)
            {
                case EvaluateResultCode.False:
                    {
                        return RuleResult.BuildFalse(rule.Type);
                    }

                case EvaluateResultCode.True:
                    {
                        // NOTE: Так как предикат правила true, надо смотреть на условные коэффициенты.
                        var condFactors = rule.GetDeserializedConditionalFactors();
                        if (condFactors == null || condFactors.Length == 0)
                        {
                            // NOTE: Условных коэффициентов нет, и редерить ничего не надо
                            return RuleResult.BuildTrue(rule.Type, rule.Factor);
                        }

                        var condResults = this.CalculateConditionalFactors(condFactors, rule.Type).ToArray();
                        return RuleResult.BuildTrue(rule.Type, rule.Factor, condResults);
                    }

                case EvaluateResultCode.ConvertibleToSQL:
                    {
                        // NOTE: Так как предикат не может быть вычислен , надо смотреть на условные коэффициенты.
                        var condFactors2 = rule.GetDeserializedConditionalFactors();
                        if (condFactors2 == null || condFactors2.Length == 0)
                        {
                            return RuleResult.BuildConvertible(rule.Type, evalResult.Converter, rule.Factor);
                        }

                        var condResults2 = this.CalculateConditionalFactors(condFactors2, rule.Type).ToArray();
                        return RuleResult.BuildConvertible(rule.Type, evalResult.Converter, rule.Factor, condResults2);
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", evalResult.Code));
                    }
            }
        }

        public EvaluateResult EvaluatePredicate(filter predicate)
        {
            /*
             * NOTE: Если придеката нет, то считаем его всегда true.
             * Это необходимо для поддержки коэффициентов по умолчанию домена.
             */
            if (predicate == null)
            {
                return EvaluateResult.BuildEvaluated(true);
            }

            var strategy = this.settings.EvalStrategySelector.SelectEvalStrategy(predicate, this.settings);

            return strategy.EvaluateExt();
        }

        private IEnumerable<ConditionalResult> CalculateConditionalFactors(
            IEnumerable<ConditionalFactor> rules,
            RuleTypes baseRuleType)
        {
            foreach (var grouping in rules.GroupBy(x => x.Priority).OrderByDescending(x => x.Key))
            {
                if (grouping.Count() > 1)
                {
                    throw new InvalidPriorityException("Условные коэффициенты должны иметь уникальных приоритет");
                }

                var conditionalFactor = grouping.First();

                log.DebugFormat("Вычисляем предикат условного коэффициента: {0}", conditionalFactor);

                var predicate = conditionalFactor.GetDeserializedPredicate();

                var evalResult = this.EvaluatePredicate(predicate);

                switch (evalResult.Code)
                {
                    case EvaluateResultCode.False:
                        {
                            // NOTE: Условный коэффициент вычислен и результат false => пропускаем, нам такой совсем не интересен.
                            continue;
                        }

                    case EvaluateResultCode.True:
                        {
                            // NOTE: Условный коэффициент вычислен и результат true => возвращаем result, и это последний остальные нам не интересны.
                            yield return ConditionalResult.BuildTrue(conditionalFactor.Factor);
                            break;
                        }

                    case EvaluateResultCode.ConvertibleToSQL:
                        {
                            // NOTE: Условный коэффициент должен быть преобразован в SQL.
                            yield return ConditionalResult.BuildConvertible(baseRuleType, evalResult.Converter, conditionalFactor.Factor);
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", evalResult.Code));
                        }
                }
            }
        }
    }
}