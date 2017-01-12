namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    /// <summary>
	/// Представляет результат вычисления группы правил.
	/// </summary>
	public class RuleGroupResult : IResult
	{
		/// <summary>
		/// Вычисленный коэффициент правила, так как правило может иметь условные коэффициенты <see cref="Rule.ConditionalFactor"/>.
		/// </summary>
		public decimal Factor { get; protected set; }

		/// <summary>
		/// Тип правила.
		/// </summary>
		public RuleTypes RuleType { get; protected set; }

		/// <summary>
		/// Конвертер в SQL.
		/// </summary>
		public ISqlConvert Converter { get; protected set; }

		/// <summary>
		/// Код результата вычисления.
		/// </summary>
		public EvaluateResultCode Code { get; protected set; }

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда все предикаты группы = false.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildFalse(RuleTypes ruleType)
		{
			return new RuleGroupResult { Code = EvaluateResultCode.False, RuleType = ruleType, Factor = ruleType.GetDefaultFactor(), Converter = null };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда нет ниодного конвертируемого в SQL правила.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="factor">
		/// Вычисленный коэффициент.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildTrue(RuleTypes ruleType, decimal factor)
		{
			return new RuleGroupResult { Code = EvaluateResultCode.True, RuleType = ruleType, Factor = factor, Converter = new FactorConverter(factor) };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда есть только одно конвертируемое в SQL правила.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="converter">
		/// Конвертер правила.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildConvertible(RuleTypes ruleType, ISqlConvert converter)
		{
			return new RuleGroupResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       RuleType = ruleType,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = converter
				       };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда есть агрегирующий результат вычисления правил.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="converter">
		/// Конвертер правила.
		/// </param>
		/// <param name="aggregateRuleResult">
		/// Fгрегирующий результат вычисления правил.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildConvertable(RuleTypes ruleType, ISqlConvert converter, AggregateRuleResult aggregateRuleResult)
		{
			var sqlWhenThen = converter as ISqlWhenThen;

			if (sqlWhenThen == null)
			{
				throw new RuleEvaluationException("Не правильный конвертер результат правила, конвертируемый результат правила должен быть типа ISqlWhenThen");
			}

			return new RuleGroupResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       RuleType = ruleType,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = new RuleSqlConverter(sqlWhenThen, aggregateRuleResult.Converter)
				       };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда несколько конвертируемых правил.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="whenThenResults">
		/// Конвертеры выражений WHEN x THEN y
		/// </param>
		/// <param name="elseResult">
		/// Результат для ветки ELSE.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildConvertable(RuleTypes ruleType, ExclusiveRuleResult[] whenThenResults, ExclusiveRuleResult elseResult)
		{
			var whenThens = whenThenResults.Select(x => x.Converter as ISqlWhenThen).ToArray();

			var elseConverter = elseResult.Converter ?? new FactorConverter(elseResult.Factor);

			return new RuleGroupResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       RuleType = ruleType,
					       Factor = ruleType.GetDefaultFactor(),
						   Converter = new RuleSqlConverter(whenThens, elseConverter)
				       };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда несколько конвертируемых правил.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="whenThenResults">
		/// Конвертеры выражений WHEN x THEN y
		/// </param>
		/// <param name="elseResult">
		/// Результат для ветки ELSE.
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildConvertable(RuleTypes ruleType, ExclusiveRuleResult[] whenThenResults, AggregateRuleResult elseResult)
		{
			var whenThens = whenThenResults.Select(x => x.Converter as ISqlWhenThen).ToArray();

			return new RuleGroupResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       RuleType = ruleType,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = new RuleSqlConverter(whenThens, elseResult.Converter)
				       };
		}

		/// <summary>
		/// Статический конструктор результата вычисления группы правил, когда несколько конвертируемых правил.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правила.
		/// </param>
		/// <param name="ruleResults">
		/// Конвертеры выражений WHEN x THEN y
		/// </param>
		/// <returns>
		/// Результат вычисления группы правил.
		/// </returns>
		public static RuleGroupResult BuildConvertible(RuleTypes ruleType, RuleResult[] ruleResults)
		{
			var trueResult = ruleResults.FirstOrDefault(x => x.Code == EvaluateResultCode.True);

			var whenThens =
				ruleResults.TakeWhile(x => x.Code == EvaluateResultCode.ConvertibleToSQL)
						   .Select(x => new SqlWhenThen(x.PredicateSqlConverter, x.Converter.WhenThen.Single().Then))
				           .ToArray();

			if (trueResult != null)
			{
				if (trueResult.IsFactorAvaible)
				{
					return new RuleGroupResult
						       {
							       Code = EvaluateResultCode.ConvertibleToSQL,
							       RuleType = ruleType,
							       Factor = ruleType.GetDefaultFactor(),
								   Converter = new RuleSqlConverter(whenThens, trueResult.Factor)
						       };
				}

				return new RuleGroupResult
					       {
						       Code = EvaluateResultCode.ConvertibleToSQL,
						       RuleType = ruleType,
						       Factor = ruleType.GetDefaultFactor(),
						       Converter = new RuleSqlConverter(whenThens, trueResult.Converter)
					       };
			}

			return new RuleGroupResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       RuleType = ruleType,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = new RuleSqlConverter(whenThens, ruleType.GetDefaultFactor())
				       };
		}
	}
}