namespace RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates
{
	using System;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
	using RapidSoft.Loaylty.PromoAction.Tracer;

    /// <summary>
	/// Агрегирующий калькулятор.
	/// </summary>
	internal class AggregateCalculator
	{
		/// <summary>
		/// Фильтр правил.
		/// </summary>
		private readonly Func<Rule, bool> filter;

		/// <summary>
		/// Калькулятор правил.
		/// </summary>
		private readonly IRuleCalculator ruleCalculator;

		/// <summary>
		/// Тип правил вычисляемый калькулятором.
		/// </summary>
		private readonly RuleTypes ruleType;

		/// <summary>
		/// Initializes a new instance of the <see cref="AggregateCalculator"/> class.
		/// </summary>
		/// <param name="ruleType">
		/// Тип правил.
		/// </param>
		/// <param name="filter">
		/// Фильтр правил.
		/// </param>
		/// <param name="settings">
		/// Настройки вычисления.
		/// </param>
		public AggregateCalculator(RuleTypes ruleType, Func<Rule, bool> filter, EvaluationSettings settings)
		{
			filter.ThrowIfNull("filter");
			settings.ThrowIfNull("settings");

			this.filter = filter;
			this.ruleType = ruleType;
			this.ruleCalculator = settings.RuleCalculator;
		}

		/// <summary>
		/// Вычисляет агрегированный результат набор правил.
		/// </summary>
		/// <param name="rules">
		/// Набор правил.
		/// </param>
		/// <returns>
		/// Агрегированный результат набора правил.
		/// </returns>
		public AggregateRuleResult Calculate(Rule[] rules)
		{
			rules.ThrowIfNull("rules");

			var selectedRules = rules.Where(rule => rule.Type == this.ruleType && this.filter(rule));

			// var results = selectedRules.Select(this.CalculateRule).ToArray();
			var results = selectedRules.Select(this.ruleCalculator.CalculateRule).ToArray();

			var retVal = AggregateRuleResult.Build(this.ruleType, results);

		    return retVal;
		}

		///// <summary>
		///// Вычисляет результат правила.
		///// </summary>
		///// <param name="rule">
		///// Вычисляемое правило.
		///// </param>
		///// <returns>
		///// Результат вычисления правила.
		///// </returns>
		//private RuleResult CalculateRule(Rule rule)
		//{
		//    return this.ruleCalculator.CalculateRule(rule);
		//}
	}
}
