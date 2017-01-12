namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
	using System.Collections.Generic;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Tracer;

	/// <summary>
	/// Класс-обертка настроек вычисления, содержит контекст и алиасы, реализацию логики вычисления правила, а также селектор логики вычисления уравнения.
	/// Если последнии два объекта не передаются через контсруктор, то класс конструирует объекты по умолчанию.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// Объект реализующий интерфейс <see cref="IEvalStrategySelector"/>.
		/// </summary>
		private IEvalStrategySelector evalStrategySelector;

	    private IVariableResolver variableResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="Settings"/> class.
		/// </summary>
		/// <param name="context">
		/// Контекст для вычисления, коллекция пар «переменная-значение».
		/// </param>
		/// <param name="aliases">
		/// Контекст для вычисления, коллекция пар «переменная-алиас столбца».
		/// </param>
		/// <param name="evalStrategySelector">
		/// The eval strategy selector.
		/// </param>
		public Settings(
			ITracer tracer,
			IDictionary<string, string> context = null,
			IDictionary<string, string> aliases = null,
			IEvalStrategySelector evalStrategySelector = null)
		{
			tracer.ThrowIfNull("tracer");
			this.Context = context;
			this.Aliases = aliases;
			this.evalStrategySelector = evalStrategySelector;
			this.Tracer = tracer;
		}

		/// <summary>
		/// Контекст для вычисления, коллекция пар «переменная-значение».
		/// </summary>
		public IDictionary<string, string> Context { get; private set; }

		/// <summary>
		/// Контекст для вычисления, коллекция пар «переменная-алиас столбца».
		/// </summary>
		public IDictionary<string, string> Aliases { get; private set; }

		public IVariableResolver VariableResolver
		{
			get
			{
				return this.variableResolver ?? (this.variableResolver = new VariableResolver(this.Context, this.Aliases));
			}
		}

		/// <summary>
		/// Объект реализующий интерфейс <see cref="IEvalStrategySelector"/>.
		/// </summary>
		public IEvalStrategySelector EvalStrategySelector
		{
			get
			{
				return this.evalStrategySelector ?? (this.evalStrategySelector = new EvalStrategySelector(this.VariableResolver));
			}
		}

	    public ITracer Tracer { get; private set; }
	}
}
