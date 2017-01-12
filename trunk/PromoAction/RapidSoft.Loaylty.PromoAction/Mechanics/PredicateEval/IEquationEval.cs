namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
	/// <summary>
	/// Интерфейс вычисления уравнения/объединения.
	/// </summary>
	public interface IEquationEval
	{
		/// <summary>
		/// Выполняет вычисление уравнения/объединения.
		/// </summary>
		/// <returns>
		/// Результат вычисления уравнения/объединения.
		/// </returns>
		bool Evaluate();

		/// <summary>
		/// Вычисляет выражение и возвращает результат указывающий вычислено ли выражение или поддерживающее оптимизирующую трансформацию в SQL.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		EvaluateResult EvaluateExt();
	}
}