using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Rapidsoft.VTB24.Reports.IisLogs.Scanner.Models;
using log4net;

namespace Rapidsoft.VTB24.Reports.IisLogs.Scanner
{
    public class IisLogsScanner : IIisLogsScanner
    {
        private static readonly Regex FileNameRegex = new Regex(@"^u_ex([0-9]{6})\.log$");

        private readonly ILog _log = LogManager.GetLogger(typeof(IisLogsScanner));

        public IEnumerable<string> GetLogDirNames()
        {
            var paths = ConfigurationManager.AppSettings["vtb24:iis_log_paths"];

            if (string.IsNullOrWhiteSpace(paths))
            {
                _log.Error("не задан путь к логам IIS");
                return Enumerable.Empty<string>();
            }

            return paths.Split(';');
        }

        public IEnumerable<string> GetLogFileNames(string dirName, DateTime fromDate, DateTime toDate)
        {
            var dirInfo = new DirectoryInfo(dirName);
            foreach (var fileInfo in dirInfo.EnumerateFiles("u_ex*.log").ToArray())
            {
                var match = FileNameRegex.Match(fileInfo.Name);
                if (!match.Success)
                {
                    continue;
                }

                DateTime logDate;
                if (!DateTime.TryParseExact(match.Groups[1].Value,
                                            "yyMMdd",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.AssumeUniversal,
                                            out logDate))
                {
                    _log.Warn("не удалось распознать дату лога: " + fileInfo.Name);
                    continue;
                }

                if (logDate.Date < fromDate.Date || logDate.Date > toDate.Date)
                {
                    continue;
                }

                _log.Info("обработка файла " + fileInfo.FullName);

                yield return fileInfo.FullName;
            }
        }

        public IEnumerable<string> GetLogFileNames(DateTime fromDate, DateTime toDate)
        {
            return GetLogDirNames().SelectMany(dirName => GetLogFileNames(dirName, fromDate, toDate));
        }

        public IEnumerable<IisLogLine> ScanLogs(DateTime fromDate, DateTime toDate)
        {
            return GetLogFileNames(fromDate, toDate)
                .SelectMany(fileName => File.ReadLines(fileName)
                                            .Select((line, lineNumber) => new IisLogLine(fileName, lineNumber, line)));
        }

        public IEnumerable<IisLogLine> ScanLogs(DateTime fromDate, DateTime toDate, Regex test)
        {
            return ScanLogs(fromDate, toDate).Select(line => test.Match(line.Line).Success ? line : null);
        }
    }
}
