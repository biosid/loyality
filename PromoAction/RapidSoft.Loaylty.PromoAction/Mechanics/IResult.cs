namespace RapidSoft.Loaylty.PromoAction.Mechanics
{
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
	/// Интерфейс результат правила или группы правил.
	/// </summary>
	public interface IResult
	{
		/// <summary>
		/// Код результата вычисления.
		/// </summary>
		EvaluateResultCode Code { get; }
	}
}