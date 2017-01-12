namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;

	/// <summary>
	/// Конвертер для элемента шаблона "csvexception".
	/// </summary>
	public class CsvExceptionPatternConverter : CsvPatternConverter
	{
		/// <summary>
		/// Метод записи данных в поток отладочной информации для элемента шаблона "csvexception".
		/// </summary>
		/// <param name="writer">
		/// Реализация <see cref="TextWriter"/> для записи строк в формате CSV.
		/// </param>
		/// <param name="event">
		/// Отладочное сообщение.
		/// </param>
		protected override void WriteData(CsvTextWriter writer, LoggingEvent @event)
		{
			var exceptionData = @event.GetExceptionString();

			if (string.IsNullOrEmpty(exceptionData))
			{
				return;
			}

			writer.WriteQuote();
			writer.Write(exceptionData);
			writer.WriteQuote();
		}
	}
}