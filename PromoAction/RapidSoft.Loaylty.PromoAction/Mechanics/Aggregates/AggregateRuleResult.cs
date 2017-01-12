namespace RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    /// <summary>
    /// Представляет агрегированный результат набора правил.
    /// </summary>
    public class AggregateRuleResult : IResult
    {
        /// <summary>
        /// Тип правила.
        /// </summary>
        private readonly RuleTypes ruleType;

        /// <summary>
        /// Коллекция конвертеров правил.
        /// </summary>
        private readonly List<IRuleSqlConverter> aggregateСonverters = new List<IRuleSqlConverter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRuleResult"/> class.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        private AggregateRuleResult(RuleTypes ruleType)
        {
            this.ruleType = ruleType;
            this.Code = EvaluateResultCode.False;
            this.AggregateFactor = ruleType.GetDefaultFactor();
        }

        /// <summary>
        /// Вычисленный коэффициент набора правила, то есть, в зависимости от типа правил, 
        /// произведение или сумма коэффициентов правил с предикатом вычисленным = <c>true</c>.
        /// </summary>
        public decimal AggregateFactor { get; private set; }

        /// <summary>
        /// Конвертер в SQL.
        /// </summary>
        public ISqlConvert Converter
        {
            get
            {
                if (this.aggregateСonverters.Count == 0)
                {
                    return new FactorConverter(this.AggregateFactor);
                }

                return new AggregateConverter(this.ruleType, this.AggregateFactor) { this.aggregateСonverters };
            }
        }

        /// <summary>
        /// Код результата вычисления.
        /// </summary>
        public EvaluateResultCode Code { get; private set; }

        /// <summary>
        /// Статический конструктор агрегирующего результата.
        /// </summary>
        /// <param name="ruleTypes">
        /// Тип правила.
        /// </param>
        /// <param name="factor">
        /// Коэффициент правила.
        /// </param>
        /// <returns>
        /// Агрегирующий результат.
        /// </returns>
        public static AggregateRuleResult Build(RuleTypes ruleTypes, decimal factor)
        {
            var retVal = new AggregateRuleResult(ruleTypes);

            retVal.Add(factor);

            return retVal;
        }

        /// <summary>
        /// Статический конструктор агрегирующего результата.
        /// </summary>
        /// <param name="ruleTypes">
        /// Тип правила.
        /// </param>
        /// <param name="results">
        /// Набор результатов правил.
        /// </param>
        /// <returns>
        /// Агрегирующий результат.
        /// </returns>
        public static AggregateRuleResult Build(RuleTypes ruleTypes, params RuleResult[] results)
        {
            var retVal = new AggregateRuleResult(ruleTypes);

            retVal.Add(results);

            return retVal;
        }

        /// <summary>
        /// Статический конструктор агрегирующего результата.
        /// </summary>
        /// <param name="ruleTypes">
        /// Тип правила.
        /// </param>
        /// <param name="aggregateRuleResults">
        /// Набор агрегирующих результатов правил.
        /// </param>
        /// <returns>
        /// Агрегирующий результат.
        /// </returns>
        public static AggregateRuleResult Build(RuleTypes ruleTypes, params AggregateRuleResult[] aggregateRuleResults)
        {
            var retVal = new AggregateRuleResult(ruleTypes);

            retVal.Add(aggregateRuleResults);

            return retVal;
        }
        
        /// <summary>
        /// Статический конструктор агрегирующего результата.
        /// </summary>
        /// <param name="ruleTypes">
        /// Тип правила.
        /// </param>
        /// <param name="aggregateRuleResults">
        /// Набор агрегирующих результатов правил.
        /// </param>
        /// <returns>
        /// Агрегирующий результат.
        /// </returns>
        public static AggregateRuleResult Build(RuleTypes ruleTypes, IEnumerable<AggregateRuleResult> aggregateRuleResults)
        {
            var retVal = new AggregateRuleResult(ruleTypes);

            retVal.Add(aggregateRuleResults);

            return retVal;
        }

        /// <summary>
        /// Статический конструктор агрегирующего результата.
        /// </summary>
        /// <param name="ruleTypes">
        /// Тип правила.
        /// </param>
        /// <param name="ruleResult">
        /// Pезультат правила.
        /// </param>
        /// <param name="aggregateRuleResults">
        /// Набор агрегирующих результатов правил.
        /// </param>
        /// <returns>
        /// Агрегирующий результат.
        /// </returns>
        public static AggregateRuleResult Build(RuleTypes ruleTypes, RuleResult ruleResult, params AggregateRuleResult[] aggregateRuleResults)
        {
            var retVal = new AggregateRuleResult(ruleTypes);

            retVal.Add(ruleResult);

            retVal.Add(aggregateRuleResults);

            return retVal;
        }

        /// <summary>
        /// Добавляет коэффициент.
        /// </summary>
        /// <param name="factor">
        /// Добавляемфй коэффициент.
        /// </param>
        private void Add(decimal factor)
        {
            this.AggregateFactor = this.ruleType.Aggregate(this.AggregateFactor, factor);

            if (this.Code == EvaluateResultCode.False)
            {
                this.Code = EvaluateResultCode.True;
            }
        }

        /// <summary>
        /// Добавляет конвертера правила.
        /// </summary>
        /// <param name="converter">
        /// Конвертера правила.
        /// </param>
        private void Add(IRuleSqlConverter converter)
        {
            if (converter == null)
            {
                return;
            }

            this.Code = EvaluateResultCode.ConvertibleToSQL;
            this.aggregateСonverters.Add(converter);
        }

        /// <summary>
        /// Добавляет набор конвертеров правил.
        /// </summary>
        /// <param name="converters">
        /// Набор конвертеров правил.
        /// </param>
        private void Add(IEnumerable<IRuleSqlConverter> converters)
        {
            if (converters == null)
            {
                return;
            }

            foreach (var ruleSqlConverter in converters)
            {
                this.Add(ruleSqlConverter);
            }
        }

        /// <summary>
        /// Добавляет результат правила.
        /// </summary>
        /// <param name="result">
        /// Результат правила.
        /// </param>
        private void Add(RuleResult result)
        {
            if (result == null)
            {
                return;
            }

            switch (result.Code)
            {
                case EvaluateResultCode.False:
                    {
                        return;
                    }

                case EvaluateResultCode.True:
                    {
                        if (result.IsFactorAvaible)
                        {
                            this.Add(result.Factor);
                        }
                        else
                        {
                            this.Add(result.Converter);
                        }

                        return;
                    }

                case EvaluateResultCode.ConvertibleToSQL:
                    {
                        this.Add(result.Converter);
                        return;
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", result.Code));
                    }
            }
        }

        /// <summary>
        /// Добавляет набор результатов правил.
        /// </summary>
        /// <param name="results">
        /// Набор результатов правил.
        /// </param>
        private void Add(IEnumerable<RuleResult> results)
        {
            if (results == null)
            {
                return;
            }

            foreach (var ruleResult in results)
            {
                this.Add(ruleResult);
            }
        }

        /// <summary>
        /// Добавляет агрегированный результат.
        /// </summary>
        /// <param name="result">
        /// Агрегированный результат.
        /// </param>
        private void Add(AggregateRuleResult result)
        {
            if (result == null)
            {
                return;
            }

            switch (result.Code)
            {
                case EvaluateResultCode.False:
                    {
                        return;
                    }

                case EvaluateResultCode.True:
                    {
                        this.Add(result.AggregateFactor);
                        return;
                    }

                case EvaluateResultCode.ConvertibleToSQL:
                    {
                        this.Add(result.AggregateFactor);
                        this.Add(result.aggregateСonverters);
                        return;
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", result.Code));
                    }
            }
        }

        /// <summary>
        /// Добавляет набор агрегированных результатов.
        /// </summary>
        /// <param name="results">
        /// Набор агрегированных результатов.
        /// </param>
        private void Add(IEnumerable<AggregateRuleResult> results)
        {
            if (results == null)
            {
                return;
            }

            foreach (var result in results)
            {
                this.Add(result);
            }
        }
    }
}
