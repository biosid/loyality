namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
	/// Реализация проверки не эквивалентности входных значений.
	/// </summary>
	internal class NotEqEquation : EquationEvalBase
	{
        private readonly ILog log = LogManager.GetLogger(typeof(NotEqEquation));

        /// <summary>
		/// Initializes a new instance of the <see cref="NotEqEquation"/> class.
		/// </summary>
		/// <param name="values">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <param name="factory">
		/// Фабрика конвертеров.
		/// </param>
		public NotEqEquation(VariableValue[] values, IEvalStrategySelector factory)
			: base(values, factory)
		{
		}

		/// <summary>
		/// Выполняет проверку не эквивалентности входных значений.
		/// Кол-во входный значений должно быть равно 2.
		/// </summary>
		/// <param name="objects">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected override bool DoEval(object[] objects)
		{
			log.Debug("Вычисляем проверку не эквивалентности.");
			if (objects.Length != 2)
			{
				throw new InvalidPredicateFormatException("Уравнение проверки на не эквивалентность должно содержать 2 переменных/литерала");
			}

			if (objects[0] == null || objects[1] == null)
			{
				return false;
			}

			return !objects[0].Equals(objects[1]);
		}

		/// <summary>
		/// Выполняет проверку не эквивалентности или трансформацию в SQL проверки входных значений.
		/// Кол-во входный значений должно быть равно 2.
		/// </summary>
		/// <param name="values">
		/// Коллекция значений переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected override EvaluateResult DoEvalExt(VariableValue[] values)
		{
			log.Debug("Формируем SQL проверки эквивалентности.");
			if (values.Length != 2)
			{
				throw new InvalidPredicateFormatException("Уравнение проверки на эквивалентность должно содержать 2 переменных/литерала");
			}

			return EvaluateResult.BuildTansformed(this.Factory.CreateEquationConverter(equationOperator.noteq, values));
		}
	}
}