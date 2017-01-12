namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
	using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
	using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    /// <summary>
	/// Вычислятель исключающих правил.
	/// </summary>
	public class ExclusiveCalculator : IRuleGroupCalculator
	{
		/// <summary>
		/// Настройки вычисления.
		/// </summary>
		private readonly EvaluationSettings settings;

		/// <summary>
		/// Настройки вычисления с кэширующим калькулятором правил.
		/// </summary>
		private readonly EvaluationSettings settingsWithCashedResult;

		/// <summary>
		/// Тип правил вычисляемый калькулятором.
		/// </summary>
		private readonly RuleTypes ruleType;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExclusiveCalculator"/> class.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правил.
		/// </param>
		/// <param name="settings">
		/// Настройки вычисления.
		/// </param>
		public ExclusiveCalculator(
			RuleTypes ruleType, 
			EvaluationSettings settings)
		{
			this.settings = settings;
			if (ruleType == RuleTypes.BaseAddition || ruleType == RuleTypes.BaseMultiplication)
			{
				throw new InvalidRuleGroupTypeException("Калькулятор исключительных правил может вычислять только не базовые правила");
			}

			this.settingsWithCashedResult = new EvaluationSettings(
				settings.Tracer,
				settings.Context,
				settings.Aliases,
				new CachedRuleCalculator(settings.Tracer, settings.RuleCalculator),
				settings.EvalStrategySelector);

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

			var groupedRules = rulesByType.Where(x => x.IsExclusive).GroupBy(r => r.Priority).OrderByDescending(x => x.Key);

			var exlusiveResults = groupedRules.Select(x => this.CalculateRuleGroup(x, rulesByType)).TakeToFirstTrue().ToArray();

			if (exlusiveResults.Length == 0)
			{
				// NOTE: Вычисление всех не исключающих правил
				var notExclusiveResult = this.CalculateNotExclusiveResult(rulesByType);

				switch (notExclusiveResult.Code)
				{
					case EvaluateResultCode.False:
						{
							return RuleGroupResult.BuildFalse(this.ruleType);
						}

					case EvaluateResultCode.True:
						{
							return RuleGroupResult.BuildTrue(this.ruleType, notExclusiveResult.AggregateFactor);
						}

					case EvaluateResultCode.ConvertibleToSQL:
						{
							return RuleGroupResult.BuildConvertible(this.ruleType, notExclusiveResult.Converter);
						}

					default:
						{
							throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", notExclusiveResult.Code));
						}
				}
			}

			if (exlusiveResults.Length == 1)
			{
				var exlusiveResult = exlusiveResults.First();
				switch (exlusiveResult.Code)
				{
					case EvaluateResultCode.False:
						{
							throw new RuleEvaluationException(
								"Коллекция результатов исключающих правил не должна содержать результат с кодом EvaluateResultCode.False");
						}

					case EvaluateResultCode.True:
						{
							// NOTE: Найдено одно исключающее правило
							return exlusiveResult.IsFactorAvaible
									   ? RuleGroupResult.BuildTrue(this.ruleType, exlusiveResult.Factor)
									   : RuleGroupResult.BuildConvertible(this.ruleType, exlusiveResult.Converter);
						}

					case EvaluateResultCode.ConvertibleToSQL:
						{
							// NOTE: Найдено одно исключающее правило
							var calculateNotExclusiveExcludedBy = this.CalculateNotExclusiveAndExclusiveLowPriority(
								rulesByType, exlusiveResult.RulePriority);
							return RuleGroupResult.BuildConvertable(this.ruleType, exlusiveResult.Converter, calculateNotExclusiveExcludedBy);
						}

					default:
						{
							throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается ", exlusiveResult.Code));
						}
				}
			}

			var trueResult = exlusiveResults.FirstOrDefault(x => x.Code == EvaluateResultCode.True);

			if (trueResult != null)
			{
				var convertable = exlusiveResults.Where(x => x.Code == EvaluateResultCode.ConvertibleToSQL).ToArray();
				return RuleGroupResult.BuildConvertable(this.ruleType, convertable, trueResult);
			}

			var calculateNotExclusive = this.CalculateNotExclusiveResult(rulesByType);
			return RuleGroupResult.BuildConvertable(this.ruleType, exlusiveResults, calculateNotExclusive);
		}

		/// <summary>
		/// Вычисление результата для исключающих правил сгруппированных и отсортированных по приоритету.
		/// </summary>
		/// <param name="ruleGroup">
		/// Коллекция группированных по приоритету исключающих правил.
		/// </param>
		/// <param name="rules">
		/// Все правила определенного типа, <see cref="ruleType"/>.
		/// </param>
		/// <returns>
		/// Результат вычисления исключиющего правила.
		/// </returns>
		private ExclusiveRuleResult CalculateRuleGroup(IGrouping<int, Rule> ruleGroup, Rule[] rules)
		{
			ruleGroup.ThrowIfNull("ruleGroup");

			if (ruleGroup.Count() > 1)
			{
				throw new InvalidPriorityException("Должно быть только одно правило по приоритеру");
			}

			var ruleFromGroup = ruleGroup.First();

			var ruleResult = this.settings.RuleCalculator.CalculateRule(ruleFromGroup);

			this.settings.Tracer.Trace(ruleFromGroup, ruleResult);

			switch (ruleResult.Code)
			{
				case EvaluateResultCode.False:
					{
						return ExclusiveRuleResult.BuildFalse(ruleFromGroup.Type);
					}

				case EvaluateResultCode.True:
					{
						this.settings.Tracer.Trace(
							ruleFromGroup.Type, 
							"Вычисление не исключаемых правил, когда найдено исключающее с true предикатом.");
						var notExcludedByResult = this.CalculateNotExcludedByResult(rules, ruleFromGroup.Priority);

						var aggreg = AggregateRuleResult.Build(this.ruleType, ruleResult, notExcludedByResult);

						return aggreg.Code == EvaluateResultCode.True
								   ? ExclusiveRuleResult.BuildTrue(aggreg.AggregateFactor)
								   : ExclusiveRuleResult.BuildTrue(ruleFromGroup.Type, aggreg.Converter);
					}

				case EvaluateResultCode.ConvertibleToSQL:
					{
						this.settings.Tracer.Trace(
							ruleFromGroup.Type,
							"Вычисление не исключаемых правил, когда найдено исключающее с convertible предикатом.");
						var notExcludedByResult = this.CalculateNotExcludedByResult(rules, ruleFromGroup.Priority);

						var aggreg = AggregateRuleResult.Build(this.ruleType, ruleResult, notExcludedByResult);

						return ExclusiveRuleResult.Convertible(this.ruleType, aggreg.Converter, ruleResult, ruleFromGroup.Priority);
					}

				default:
					{
						throw new NotSupportedException(string.Format("Тип результата {0} не поддерживается", ruleResult.Code));
					}
			}
		}

		/// <summary>
		/// Вычисление всех правил, без учета признака "не исключаемое", но исключаемые правила только те приоритет которых ниже заданного.
		/// Используется когда найдено конвертируемое исключающее правило, например, пусть имеем:
		/// 1. Исключающее правило с приоритетом 5.
		/// 2. Исключающее правило с приоритетом 4.
		/// 3. Исключающее правило с приоритетом 3.
		/// 4. Не исключающее правило.
		/// Пусть правило 2 не исчислимое, то есть конвертируестя в TSQL, тогда для ветки ELSE выражения CASE, необходимо, взять правила 3 и 4; 
		/// правило 1 считать не надо, так как у него приоритет больше и оно исключающее, а следовательно правило было вычислено до правила 2;
		/// правило 2 считать не надо, так как по условиям примера оно не исчислимое, то есть конвертируестя в TSQL и 
		/// его коэффициент учитывается в ветке THEN выражения CASE WHEN.
		/// </summary>
		/// <param name="rules">
		/// Коллекция правил.
		/// </param>
		/// <param name="currentRulePriority">
		/// Приоритет текущего правила.
		/// </param>
		/// <returns>
		/// Агрегированный результат набора правил.
		/// </returns>
		private AggregateRuleResult CalculateNotExclusiveAndExclusiveLowPriority(Rule[] rules, int currentRulePriority)
		{
			var calculator = new AggregateCalculator(this.ruleType, r => ((r.IsExclusive && r.Priority < currentRulePriority) || !r.IsExclusive), this.settingsWithCashedResult);

			var retVal = calculator.Calculate(rules);

			var comment = string.Format(
				"Группа правил: исключающих с приоритетом меньше {0}; всех не исключающих", currentRulePriority);
			this.settings.Tracer.Trace(this.ruleType, retVal, comment);

			return retVal;
		}

		/// <summary>
		/// Вычисление не исключающих и не исключаемых правил и исключающих, не исключаемых правил с приоритетом ниже заданного.
		/// </summary>
		/// <param name="rules">
		/// Коллекция правил.
		/// </param>
		/// <param name="currentRulePriority">
		/// Приоритет текущего правила.
		/// </param>
		/// <returns>
		/// Агрегированный результат набора правил.
		/// </returns>
		private AggregateRuleResult CalculateNotExcludedByResult(Rule[] rules, int currentRulePriority)
		{
			// NOTE: Фильтр (не исключающих и не исключаемых правил) или (исключающих, не исключаемых правил с приоритетом ниже заданного).
			Func<Rule, bool> filter =
				r => (!r.IsExclusive && r.IsNotExcludedBy) || (r.IsExclusive && r.IsNotExcludedBy && r.Priority < currentRulePriority);

			var calculator = new AggregateCalculator(this.ruleType, filter, this.settingsWithCashedResult);

			var result = calculator.Calculate(rules);

			var notExcludedByResult = AggregateRuleResult.Build(this.ruleType, result);

			var comment =
				string.Format(
					"Группа правил: не исключающих не исключаемых; исключающих не исключаемых с приоритетом меньше {0};",
					currentRulePriority);
			this.settings.Tracer.Trace(this.ruleType, notExcludedByResult, comment);

			return notExcludedByResult;
		}

		/// <summary>
		/// Вычисление не исключающих правил, без учета признака "не исключаемое".
		/// Используется когда не найдено ни одного исключающего правила.
		/// </summary>
		/// <param name="rules">
		/// Коллекция правил.
		/// </param>
		/// <returns>
		/// Агрегированный результат набора правил.
		/// </returns>
		private AggregateRuleResult CalculateNotExclusiveResult(Rule[] rules)
		{
			// NOTE: Вычисление всех не исключающих правил
			var notExclusive = new AggregateCalculator(this.ruleType, rule => !rule.IsExclusive, this.settings);
			var retVal = notExclusive.Calculate(rules);

			const string Comment = "Группа правил: не исключающие";
			this.settings.Tracer.Trace(this.ruleType, retVal, Comment);

			return retVal;
		}
	}
}