namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
    using System.Globalization;
    using System.IO;

    /// <summary>
	/// Класс содержит вспомогательные методы для формирования TSQL выражений.
	/// </summary>
	internal static class WriterExtensions
	{
		/// <summary>
		/// Запись в поток "CASE".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void Case(this TextWriter writer)
		{
			writer.Write("CASE");
		}

		/// <summary>
		/// Запись в поток " WHEN ".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void When(this TextWriter writer)
		{
			writer.Write(" WHEN ");
		}

		/// <summary>
		/// Запись в поток "CASE WHEN ".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void CaseWhen(this TextWriter writer)
		{
			writer.Write("CASE WHEN ");
		}

		/// <summary>
		/// Запись в поток " THEN ".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void Then(this TextWriter writer)
		{
			writer.Write(" THEN ");
		}

		/// <summary>
		/// Запись в поток " ELSE ".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void Else(this TextWriter writer)
		{
			writer.Write(" ELSE ");
		}

		/// <summary>
		/// Запись в поток " END".
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public static void End(this TextWriter writer)
		{
			writer.Write(" END");
		}

		/// <summary>
		/// Запись в поток коэффициента.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		/// <param name="factor">
		/// Записываемый коэффициент.
		/// </param>
		/// <param name="cultureInfo">
		/// Информация о локализации, по умолчанию <see cref="CultureInfo.InvariantCulture"/>.
		/// </param>
		public static void Factor(this TextWriter writer, decimal factor, CultureInfo cultureInfo = null)
		{
			writer.Write(factor.ToString(cultureInfo ?? CultureInfo.InvariantCulture).TrimDecimal());
		}

		/// <summary>
		/// Удаляет лишние нули после запятой:
		/// 1000.000 -> 1000
		/// 0.500 -> 0.5
		/// </summary>
		/// <param name="str">
		/// Обрататываемая строка.
		/// </param>
		/// <returns>
		/// Результирующая строка.
		/// </returns>
		public static string TrimDecimal(this string str)
		{
			var value = str.IndexOf('.') == -1 ? str : str.TrimEnd('0').TrimEnd('.');
			return value == string.Empty ? "0" : value;
		}
	}
}
