namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
	/// Интерфейс конвертера в выражение WHEN ПРЕДИКАТ THEN ВЫРАЖЕНИЕ.
	/// </summary>
	public interface ISqlWhenThen : ISqlConvert
	{
		/// <summary>
		/// Предикат выражения.
		/// </summary>
		IPredicateSqlConverter PredicateSqlConverter { get; }

		/// <summary>
		/// Ветка then выражения.
		/// </summary>
		ISqlConvert Then { get; }
	}
}