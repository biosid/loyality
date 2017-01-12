namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;

	/// <summary>
	/// Конвертер для элемента шаблона "csvmessage".
	/// </summary>
	public class CsvMessagePatternConverter : CsvPatternConverter
	{
		/// <summary>
		/// Метод записи данных в поток отладочной информации для элемента шаблона "csvmessage".
		/// </summary>
		/// <param name="writer">
		/// Реализация <see cref="TextWriter"/> для записи строк в формате CSV.
		/// </param>
		/// <param name="event">
		/// Отладочное сообщение.
		/// </param>
		protected override void WriteData(CsvTextWriter writer, LoggingEvent @event)
		{
			var message = @event.RenderedMessage;

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			writer.WriteQuote();
			writer.Write(message);
			writer.WriteQuote();
		}
	}
}