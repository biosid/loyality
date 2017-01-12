namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;
	using log4net.Layout;

	using RapidSoft.Extensions;

	/// <summary>
	/// Реализация <see cref="TextWriter"/> для записи отладочной информации в формате CSV.
	/// </summary>
	public sealed class CsvPatternLayout : PatternLayout
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CsvPatternLayout"/> class.
		/// Переопределяет запись ошибки по умолчанию.
		/// </summary>
		public CsvPatternLayout()
		{
			this.IgnoresException = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvPatternLayout"/> class.
		/// Переопределяет запись ошибки по умолчанию.
		/// </summary>
		/// <param name="pattern">
		/// The pattern.
		/// </param>
		public CsvPatternLayout(string pattern)
			: base(pattern)
		{
			this.IgnoresException = false;
		}

		/// <summary>
		/// Initialize layout options.
		/// Добавляет конверторы для элементов шаблона "csvexception" и "csvmessage"
		/// </summary>
		public override void ActivateOptions()
		{
			this.AddConverter("csvexception", typeof(CsvExceptionPatternConverter));
			this.AddConverter("csvmessage", typeof(CsvMessagePatternConverter));
			base.ActivateOptions();
		}

		/// <summary>
		/// Produces a formatted string as specified by the conversion pattern.
		/// Оборачивает <paramref name="writer"/> в <see cref="CsvTextWriter"/>
		/// </summary>
		/// <param name="writer">
		/// The TextWriter to write the formatted event to
		/// </param>
		/// <param name="loggingEvent">
		/// the event being logged
		/// </param>
		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.ThrowIfNull("writer");

			var ctw = new CsvTextWriter(writer);
			base.Format(ctw, loggingEvent);
		}
	}
}