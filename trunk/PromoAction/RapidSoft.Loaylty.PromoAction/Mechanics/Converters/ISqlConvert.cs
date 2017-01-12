namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
    using System.IO;

    /// <summary>
	/// Интерфейс формирования Sql выражения, результатом вычисления которого является число.
	/// </summary>
	public interface ISqlConvert
	{
		/// <summary>
		/// Записывает некоторое выражение в поток как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		void Convert(TextWriter writer);
	}
}
