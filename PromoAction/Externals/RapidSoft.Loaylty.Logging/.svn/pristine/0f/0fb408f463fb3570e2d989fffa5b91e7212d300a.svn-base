namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;
	using log4net.Util;

	/// <summary>
	/// Базовый конвертер для элементов шаблона "csvexception" и "csvmessage".
	/// </summary>
	public abstract class CsvPatternConverter : PatternConverter
	{
		/// <summary>
		/// Evaluate this pattern converter and write the output to a writer.
		/// </summary>
		/// <param name="writer">
		/// <see cref="T:System.IO.TextWriter"/> that will receive the formatted result.
		/// </param>
		/// <param name="state">
		/// The state object on which the pattern converter should be executed.
		/// </param>
		protected override void Convert(TextWriter writer, object state)
		{
			var loggingEvent = state as LoggingEvent;

			if (loggingEvent == null)
			{
				return;
			}

			var csvTextWriter = writer as CsvTextWriter;

			if (csvTextWriter == null)
			{
				return;
			}

			this.WriteData(csvTextWriter, loggingEvent);
		}

		/// <summary>
		/// Метод записи данных в поток отладочной информации для элементов шаблона "csvexception" и "csvmessage".
		/// </summary>
		/// <param name="writer">
		/// Реализация <see cref="TextWriter"/> для записи строк в формате CSV.
		/// </param>
		/// <param name="event">
		/// Отладочное сообщение.
		/// </param>
		protected abstract void WriteData(CsvTextWriter writer, LoggingEvent @event);
	}
}