namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;
	using System.Text;

	using RapidSoft.Extensions;

	/// <summary>
	/// Реализация <see cref="TextWriter"/> для записи строк в формате CSV: задвоение кавычек.
	/// </summary>
	public class CsvTextWriter : TextWriter
	{
		/// <summary>
		/// Оригинальный <see cref="TextWriter"/>.
		/// </summary>
		private readonly TextWriter textWriter;

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvTextWriter"/> class.
		/// </summary>
		/// <param name="textWriter">
		/// The text writer.
		/// </param>
		public CsvTextWriter(TextWriter textWriter)
		{
			textWriter.ThrowIfNull("textWriter");
			this.textWriter = textWriter;
		}

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		public override Encoding Encoding
		{
			get { return this.textWriter.Encoding; }
		}

		/// <summary>
		/// Writes a character to the text string or stream.
		/// Задваивает кавычку.
		/// </summary>
		/// <param name="value">
		/// The character to write to the text stream. </param>
		public override void Write(char value)
		{
			this.textWriter.Write(value);
			if (value == '"')
			{
				this.textWriter.Write(value);
			}
		}

		/// <summary>
		/// Записывает кавычку в поток без задвоения.
		/// </summary>
		public void WriteQuote()
		{
			this.textWriter.Write('"');
		}
	}
}
