namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
	/// <summary>
	/// Коды результатов для реализации логики когда результат вычисления может принимать значения <c>true</c>, <c>false</c>и "необходимо конвертировать в SQL".
	/// </summary>
	public enum EvaluateResultCode
	{
		/// <summary>
		/// Результата вычисления <c>true</c>.
		/// </summary>
		True, 

		/// <summary>
		/// Результата вычисления <c>false</c>.
		/// </summary>
		False,

		/// <summary>
		/// Результат вычисления необходимо конвертировать в SQL.
		/// </summary>
		ConvertibleToSQL
	}
}