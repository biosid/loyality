namespace RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator
{
    using System.IO;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
    /// Представляет результат вычисления правила.
    /// </summary>
    public class RuleResult : IResult
    {
        /// <summary>
        /// Вычисленный коэффициент правила, так как правило может иметь условные коэффициенты <see cref="ConditionalFactor"/>.
        /// </summary>
        public decimal Factor { get; protected set; }

        /// <summary>
        /// Признак указывающий доступен ли коэффициент, так как предикат правила может быть true, 
        /// но условные коэффициенты правила конвертируются в SQL
        /// </summary>
        public bool IsFactorAvaible { get; protected set; }

        /// <summary>
        /// Конвертер предикат, <c>null</c>, если <see cref="Code"/> не равен <see cref="EvaluateResultCode.ConvertibleToSQL"/>.
        /// Необходим для построения правильного CASE WHEN выражения исключающего правила.
        /// </summary>
        public IPredicateSqlConverter PredicateSqlConverter { get; protected set; }

        /// <summary>
        /// Код результата вычисления.
        /// </summary>
        public EvaluateResultCode Code { get; protected set; }

        /// <summary>
        /// Конвертер правила.
        /// </summary>
        public IRuleSqlConverter Converter { get; protected set; }

        /// <summary>
        /// Признак указывающий результат правила учитывает условные коэффициенты.
        /// </summary>
        public bool IsInclideConditionalFactors { get; protected set; }
        
        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила = false.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        public static RuleResult BuildFalse(RuleTypes ruleType)
        {
            return new RuleResult { Code = EvaluateResultCode.False, Factor = ruleType.GetDefaultFactor() };
        }

        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила = true.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила
        /// </param>
        /// <param name="factor">
        /// Коэффициент правила.
        /// </param>
        /// <param name="condRuleResults">
        /// The cond Rule Result.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        public static RuleResult BuildTrue(
            RuleTypes ruleType,
            decimal factor,
            params ConditionalResult[] condRuleResults)
        {
            var trueCondRuleResults = condRuleResults.FirstOrDefault(x => x.Code == EvaluateResultCode.True);

            var condWhens = condRuleResults.Where(x => x.Code == EvaluateResultCode.ConvertibleToSQL).Select(x => x.Converter).ToArray();

            IRuleSqlConverter converter;
            bool isInclideConditionalFactors;
            bool isFactorAvaible;
            decimal resultFactor;

            if (trueCondRuleResults != null)
            {
                // NOTE: Есть условный коэфф. с предикатом == true, => коэфф. правила уже не будет использоваться
                resultFactor = trueCondRuleResults.Factor;

                if (condWhens.Length == 0)
                {
                    converter = null;
                    isInclideConditionalFactors = false;
                    isFactorAvaible = true;
                }
                else
                {
                    converter = new RuleSqlConverter(condWhens, resultFactor);
                    isInclideConditionalFactors = true;
                    isFactorAvaible = false;
                }
            }
            else
            {
                if (condWhens.Length == 0)
                {
                    resultFactor = factor;
                    isFactorAvaible = true;
                    converter = null;
                    isInclideConditionalFactors = false;
                }
                else
                {
                    resultFactor = ruleType.GetDefaultFactor();
                    isFactorAvaible = false;
                    converter = new RuleSqlConverter(condWhens, factor);
                    isInclideConditionalFactors = true;
                }
            }

            return new RuleResult
                       {
                           Code = EvaluateResultCode.True,
                           Factor = resultFactor,
                           Converter = converter,
                           IsFactorAvaible = isFactorAvaible,
                           IsInclideConditionalFactors = isInclideConditionalFactors,
                           PredicateSqlConverter = null
                       };
        }

        /// <summary>
        /// Статический конструктор результата вычисления правила, когда предикат правила конвертируем в SQL.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <param name="predicate">
        /// Конвертер предиката.
        /// </param>
        /// <param name="factor">
        /// Коэффициент правила.
        /// </param>
        /// <param name="condRuleResults">
        /// Коллекция результатов условных коэффициентов.
        /// </param>
        /// <returns>
        /// Результат вычисления правила.
        /// </returns>
        public static RuleResult BuildConvertible(
            RuleTypes ruleType,
            IPredicateSqlConverter predicate,
            decimal factor, 
            params ConditionalResult[] condRuleResults)
        {
            var defaultFactor = ruleType.GetDefaultFactor();

            var trueCondRuleResults = condRuleResults.FirstOrDefault(x => x.Code == EvaluateResultCode.True);

            var condWhens = condRuleResults.Where(x => x.Code == EvaluateResultCode.ConvertibleToSQL).Select(x => x.Converter).ToArray();

            IRuleSqlConverter convert;
            bool isInclideConditionalFactors;

            if (trueCondRuleResults != null)
            {
                // NOTE: Есть условный коэфф. с предикатом == true, => коэфф. правила уже не будет использоваться
                var resultFactor = trueCondRuleResults.Factor;

                if (condWhens.Length == 0)
                {
                    convert = new RuleSqlConverter(new SqlWhenThen(predicate, resultFactor), defaultFactor);
                    isInclideConditionalFactors = false;
                }
                else
                {
                    var thenConverter = new RuleSqlConverter(condWhens, resultFactor);
                    convert = new RuleSqlConverter(new SqlWhenThen(predicate, thenConverter), defaultFactor);
                    isInclideConditionalFactors = true;
                }
            }
            else
            {
                if (condWhens.Length == 0)
                {
                    convert = new RuleSqlConverter(new SqlWhenThen(predicate, factor), defaultFactor);
                    isInclideConditionalFactors = false;
                }
                else
                {
                    var thenConverter = new RuleSqlConverter(condWhens, factor);
                    convert = new RuleSqlConverter(new SqlWhenThen(predicate, thenConverter), defaultFactor);
                    isInclideConditionalFactors = true;
                }
            }

            return new RuleResult
                       {
                           Code = EvaluateResultCode.ConvertibleToSQL,
                           Factor = defaultFactor,
                           Converter = convert,
                           IsFactorAvaible = false,
                           IsInclideConditionalFactors = isInclideConditionalFactors,
                           PredicateSqlConverter = predicate
                       };
        }

        /// <summary>
        /// Записывает предикат как выражение SQL.
        /// </summary>
        /// <param name="writer">
        /// Поток в который выполняется запись.
        /// </param>
        public void Convert(TextWriter writer)
        {
            if (this.Converter == null)
            {
                new FactorConverter(this.Factor).Convert(writer);
                return;
            }

            this.Converter.Convert(writer);
        }
    }
}