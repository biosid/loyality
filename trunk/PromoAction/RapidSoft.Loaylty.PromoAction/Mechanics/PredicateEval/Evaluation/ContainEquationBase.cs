namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
	/// Базовая реализация для проверки вхождения или не вхождения значения переменной в набор значений.
	/// </summary>
	internal abstract class ContainEquationBase : IEquationEval
	{
		protected ContainEquationBase(VariableValue[] values)
		{
			this.Values = values;
		}

		/// <summary>
		/// Коллекция переменных уравнения/предиката.
		/// </summary>
		protected VariableValue[] Values { get; private set; }

		/// <summary>
		/// Выполняет вычисление уравнения/объединения.
		/// </summary>
		/// <returns>
		/// Результат вычисления уравнения/объединения.
		/// </returns>
		public abstract bool Evaluate();

		/// <summary>
		/// Вычисляет выражение и возвращает результат указывающий вычислено ли выражение или поддерживающее оптимизирующую трансформацию в SQL.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		public EvaluateResult EvaluateExt()
		{
			var result = this.Evaluate();

			return EvaluateResult.BuildEvaluated(result);
		}
	}
}