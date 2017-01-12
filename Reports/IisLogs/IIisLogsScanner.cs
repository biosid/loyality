using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rapidsoft.VTB24.Reports.IisLogs.Scanner.Models;

namespace Rapidsoft.VTB24.Reports.IisLogs
{
    public interface IIisLogsScanner
    {
        IEnumerable<string> GetLogDirNames();

        IEnumerable<string> GetLogFileNames(string dirName, DateTime fromDate, DateTime toDate);

        IEnumerable<string> GetLogFileNames(DateTime fromDate, DateTime toDate);

        IEnumerable<IisLogLine> ScanLogs(DateTime fromDate, DateTime toDate);

        IEnumerable<IisLogLine> ScanLogs(DateTime fromDate, DateTime toDate, Regex test);
    }
}
