namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
	using System.IO;

	using RapidSoft.Extensions;

	/// <summary>
	/// Реализация <see cref="ISqlConvert"/> для коэффициента.
	/// </summary>
	public class FactorConverter : ISqlConvert
	{
		/// <summary>
		/// Значение коэффициента.
		/// </summary>
		private readonly decimal factor;

		/// <summary>
		/// Initializes a new instance of the <see cref="FactorConverter"/> class.
		/// </summary>
		/// <param name="factor">
		/// Значение коэффициента.
		/// </param>
		public FactorConverter(decimal factor)
		{
			this.factor = factor;
		}

		/// <summary>
		/// Записывает правило как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public void Convert(TextWriter writer)
		{
			writer.ThrowIfNull("writer");

			writer.Factor(this.factor);
		}
	}
}