namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;

	/// <summary>
	/// ��������� ��� �������� ������� "csvmessage".
	/// </summary>
	public class CsvMessagePatternConverter : CsvPatternConverter
	{
		/// <summary>
		/// ����� ������ ������ � ����� ���������� ���������� ��� �������� ������� "csvmessage".
		/// </summary>
		/// <param name="writer">
		/// ���������� <see cref="TextWriter"/> ��� ������ ����� � ������� CSV.
		/// </param>
		/// <param name="event">
		/// ���������� ���������.
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