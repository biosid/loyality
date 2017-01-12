namespace RapidSoft.Loaylty.PromoAction.Mechanics
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tracer;

    /// <summary>
	/// Класс-обертка настроек вычисления, содержит контекст и алиасы, реализацию логики вычисления правила, а также селектор логики вычисления уравнения.
	/// Если последнии два объекта не передаются через контсруктор, то класс конструирует объекты по умолчанию.
	/// </summary>
	public class EvaluationSettings : Settings
	{
		/// <summary>
		/// Объект реализующий интерфейс <see cref="IRuleCalculator"/>.
		/// </summary>
		private IRuleCalculator ruleCalculator;

		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluationSettings"/> class.
		/// </summary>
		/// <param name="context">
		/// Контекст для вычисления, коллекция пар «переменная-значение».
		/// </param>
		/// <param name="aliases">
		/// Контекст для вычисления, коллекция пар «переменная-алиас столбца».
		/// </param>
		/// <param name="ruleCalculator">
		/// The rule calculator.
		/// </param>
		/// <param name="evalStrategySelector">
		/// The eval strategy selector.
		/// </param>
		public EvaluationSettings(
            ITracer tracer,
			IDictionary<string, string> context = null,
			IDictionary<string, string> aliases = null,
			IRuleCalculator ruleCalculator = null,
			IEvalStrategySelector evalStrategySelector = null)
            : base(tracer, context, aliases, evalStrategySelector)
		{
			this.ruleCalculator = ruleCalculator;
		}

		/// <summary>
		/// Объект реализующий интерфейс <see cref="IRuleCalculator"/>.
		/// </summary>
		public IRuleCalculator RuleCalculator
		{
			get
			{
				return this.ruleCalculator ?? (this.ruleCalculator = new RuleCalculator.RuleCalculator(this));
			}
		}
	}
}
