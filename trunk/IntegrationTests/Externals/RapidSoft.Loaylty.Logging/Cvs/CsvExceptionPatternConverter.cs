namespace RapidSoft.Loaylty.Logging.Cvs
{
	using System.IO;

	using log4net.Core;

	/// <summary>
	/// ��������� ��� �������� ������� "csvexception".
	/// </summary>
	public class CsvExceptionPatternConverter : CsvPatternConverter
	{
		/// <summary>
		/// ����� ������ ������ � ����� ���������� ���������� ��� �������� ������� "csvexception".
		/// </summary>
		/// <param name="writer">
		/// ���������� <see cref="TextWriter"/> ��� ������ ����� � ������� CSV.
		/// </param>
		/// <param name="event">
		/// ���������� ���������.
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