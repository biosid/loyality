namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
    using System.Collections.Generic;

    /// <summary>
	/// Интерфейс конвертера правила в SQL.
	/// </summary>
	public interface IRuleSqlConverter : ISqlConvert
	{
		/// <summary>
		/// Коллекция конвертеров <see cref="ISqlWhenThen"/>
		/// </summary>
		IEnumerable<ISqlWhenThen> WhenThen { get; }

		/// <summary>
		/// Конвертер выражения.
		/// </summary>
		ISqlConvert Else { get; }
	}
}