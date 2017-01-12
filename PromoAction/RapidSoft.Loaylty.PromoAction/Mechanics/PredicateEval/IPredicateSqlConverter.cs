namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using System.IO;

    /// <summary>
	/// Интерфейс конвертера предиката в SQL, то есть итоговый результат записанный в поток это SQL выражение результатом выполнения которого является <c>true</c> или <c>false</c>.
	/// </summary>
	public interface IPredicateSqlConverter
	{
		/// <summary>
		/// Записывает предикат как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		void Convert(TextWriter writer);
	}
}