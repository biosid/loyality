namespace Rapidsoft.VTB24.Reports.IisLogs.Scanner.Models
{
    public class IisLogLine
    {
        public string FileName { get; private set; }

        public int LineNumber { get; private set; }

        public string Line { get; private set; }

        public IisLogLine(string fileName, int lineNumber, string line)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            Line = line;
        }
    }
}
