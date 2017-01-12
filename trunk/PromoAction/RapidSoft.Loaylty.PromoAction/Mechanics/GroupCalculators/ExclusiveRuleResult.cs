namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    /// <summary>
    /// Представляет результат вычисления исключительного правила.
    /// </summary>
    public class ExclusiveRuleResult : IResult
    {
        /// <summary>
        /// Вычисленный коэффициент правила, так как правило может иметь условные коэффициенты <see cref="Rule.ConditionalFactor"/>.
        /// </summary>
        public decimal Factor { get; private set; }

        /// <summary>
        /// Признак указывающий доступен ли коэффициент, так как предикат правила может быть true, 
        /// но условные коэффициенты правила конвертируются в SQL
        /// </summary>
        public bool IsFactorAvaible { get; private set; }

        /// <summary>
        /// Код результата вычисления.
        /// </summary>
        public EvaluateResultCode Code { get; private set; }

        /// <summary>
        /// Приоритет правила по которому получен данных результат.
        /// </summary>
        public int RulePriority { get; private set; }

        /// <summary>
        /// Конвертер правила или набора правил.
        /// </summary>
        public ISqlConvert Converter { get; private set; }

        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила = false.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <returns>
        /// Результат вычисления.
        /// </returns>
        public static ExclusiveRuleResult BuildFalse(RuleTypes ruleType)
        {
            return new ExclusiveRuleResult
                       {
                           Code = EvaluateResultCode.False,
                           Factor = ruleType.GetDefaultFactor(),
                           Converter = new FactorConverter(ruleType.GetDefaultFactor())
                       };
        }

        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила = true.
        /// </summary>
        /// <param name="factor">
        /// Коэффициент правила.
        /// </param>
        /// <returns>
        /// Результат вычисления.
        /// </returns>
        public static ExclusiveRuleResult BuildTrue(decimal factor)
        {
            return new ExclusiveRuleResult { Code = EvaluateResultCode.True, Factor = factor, IsFactorAvaible = true };
        }

        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила = true, но результат не вычислим.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <param name="converter">
        /// Конвертер правила или нескольких правил.
        /// </param>
        /// <returns>
        /// Результат вычисления.
        /// </returns>
        public static ExclusiveRuleResult BuildTrue(RuleTypes ruleType, ISqlConvert converter)
        {
            return new ExclusiveRuleResult
                       {
                           Code = EvaluateResultCode.True,
                           IsFactorAvaible = false,
                           Factor = ruleType.GetDefaultFactor(),
                           Converter = converter
                       };
        }

        public static ExclusiveRuleResult Convertible(RuleTypes ruleType, ISqlConvert converter, RuleResult ruleResult, int rulePriority)
        {
            var rulePredicate = ruleResult.PredicateSqlConverter;

            if (rulePredicate == null)
            {
                throw new RuleEvaluationException("Для конвертируемого результата правила обязательно должен быть конвертер предиката");
            }

            var sqlWhenThen = new SqlWhenThen(rulePredicate, converter);

            return new ExclusiveRuleResult
                       {
                           Code = EvaluateResultCode.ConvertibleToSQL,
                           IsFactorAvaible = false,
                           RulePriority = rulePriority,
                           Factor = ruleType.GetDefaultFactor(),
                           Converter = sqlWhenThen
                       };
        }
    }
}