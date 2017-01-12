namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;

    /// <summary>
	/// Интерфейс вычислятеля предиката.
	/// </summary>
	public interface IPredicateCalculator
	{
		/// <summary>
		/// Вычисляет предикат.
		/// </summary>
		/// <param name="predicate">
		/// Вычислемый предикат.
		/// </param>
		/// <returns>
		/// Результат вычисления предиката.
		/// </returns>
		EvaluateResult EvaluatePredicate(filter predicate);
	}
}