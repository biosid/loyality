namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values
{
	/// <summary>
	/// Статус переменной.
	/// </summary>
	public enum VariableStatuses
	{
		/// <summary>
		/// Переменная вычиленная как литерал или полученная из контекста.
		/// </summary>
		SimplyValue,

		/// <summary>
		/// Переменная не вычисленна и содержит алиас столбца для постоения SQL.
		/// </summary>
		SqlColumn,

		/// <summary>
		/// Переменная вычислена по особому алгоритму.
		/// </summary>
		SpecialValue
	}
}